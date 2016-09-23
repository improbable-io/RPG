package improbable.behaviours.chat

import improbable.logging.Logger
import improbable.apps.{ChatBroadcastRequestMsg, ChatManager}
import improbable.chat.{ChatBroadcastRequest, ChatBroadcastRequestPayload}
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World

class ChatBroadcastRequestHandlerBehaviour(entity : Entity, logger : Logger, world : World) extends EntityBehaviour {

  override def onReady(): Unit = {
    entity.watch[ChatBroadcastRequest].onRequestChatBroadcast{
      case ChatBroadcastRequestPayload( messageContent: String) =>
        logger.info(s"onRequestChatBroadcast: messageContent=${messageContent}")
        handleChatBroadcastRequest(entity, messageContent);
    }
  }

   def handleChatBroadcastRequest(entity : Entity, messageContent : String) : Unit = {
      world.messaging.sendToApp(classOf[ChatManager].getCanonicalName, ChatBroadcastRequestMsg(entity.entityId, messageContent))
   }

}
