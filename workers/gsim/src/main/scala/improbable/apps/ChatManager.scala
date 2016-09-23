package improbable.apps

import improbable.logging.Logger
import improbable.behaviours.chat.{ReceiveAllChatBroadcastMsgs, ReceiveChatBroadcastMsg}
import improbable.papi.EntityId
import improbable.papi.world.AppWorld
import improbable.papi.world.messaging.CustomMsg
import improbable.papi.worldapp.WorldApp

import scala.collection.mutable.ListBuffer

case class RegisterChatBroadcastListenerMsg( entityId : EntityId ) extends CustomMsg
case class DeregisterChatBroadcastListenerMsg( entityId : EntityId ) extends CustomMsg
case class ChatBroadcastRequestMsg(entityId : EntityId, messageContent : String) extends CustomMsg

class ChatManager(world: AppWorld, logger: Logger) extends WorldApp {
  logger.info("world app started")

  val chatBroadcastListenerEntities : ListBuffer[EntityId] = ListBuffer.empty
  val chatBroadcastGlobalMessageList : ListBuffer[String] = ListBuffer.empty

  world.messaging.onReceive {
    // -- On client first connects
    case RegisterChatBroadcastListenerMsg(entityId : EntityId) =>
      logger.info(s"Registering broadcast listener: ${entityId}")
      chatBroadcastListenerEntities += entityId
      world.messaging.sendToEntity(entityId, ReceiveAllChatBroadcastMsgs(chatBroadcastGlobalMessageList.toList))

    // -- On client disconnect
    case DeregisterChatBroadcastListenerMsg(entityId : EntityId) =>
      logger.info(s"Deregistering broadcast listener: ${entityId}")
      chatBroadcastListenerEntities -= entityId

    // -- Broadcast a message
    case ChatBroadcastRequestMsg(entityId : EntityId,  messageContent : String) =>
      for (listenerId : EntityId <- chatBroadcastListenerEntities) {
        logger.info(s"${entityId} wants to send '${messageContent}' to ${listenerId}")
        chatBroadcastGlobalMessageList += messageContent
        world.messaging.sendToEntity(listenerId, ReceiveChatBroadcastMsg(entityId, messageContent))
      }
  }

}