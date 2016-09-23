package improbable.behaviours.chat

import improbable.logging.Logger
import improbable.apps.{ChatManager, RegisterChatBroadcastListenerMsg}
import improbable.chat.ChatBroadcastListenerWriter
import improbable.papi.EntityId
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg

case class ReceiveAllChatBroadcastMsgs(messages : List[String]) extends CustomMsg
case class ReceiveChatBroadcastMsg(source : EntityId, messageContent : String) extends CustomMsg

class ChatBroadcastListenerHandlerBehaviour(entity : Entity, logger : Logger, world : World, chatBroadcastListenerWriter: ChatBroadcastListenerWriter) extends EntityBehaviour {

  override def onReady(): Unit = {

    // Register this chat broadcast listener with the world app
    world.messaging.sendToApp(classOf[ChatManager].getCanonicalName, RegisterChatBroadcastListenerMsg(entity.entityId))

    // Listen for chat broadcasts
    world.messaging.onReceive {

      // -- Receive and replace all messages
      case ReceiveAllChatBroadcastMsgs(messages : List[String]) =>
        logger.info(s"${entity.entityId} received *all* broadcast messages")
        chatBroadcastListenerWriter.update.messageHistory(messages).finishAndSend()

      // -- Receive a single broadcast msg
      case ReceiveChatBroadcastMsg(source : EntityId, messageContent : String) =>
        logger.info(s"${entity.entityId} received broadcast message from ${source}: ${messageContent}")
        val updatedMessageHistory = chatBroadcastListenerWriter.messageHistory ++ List(messageContent)
        chatBroadcastListenerWriter.update.messageHistory(updatedMessageHistory).finishAndSend();
    }
  }
}
