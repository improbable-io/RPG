package improbable.behaviours.delegates

import improbable.chat.ChatBroadcastRequest
import improbable.corelib.util.EntityOwnerUtils
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.unity.papi.SpecificEngineConstraint

class DelegateChatBroadcastRequestToClient(entity : Entity) extends EntityBehaviour {
  override def onReady(): Unit = {
    entity.delegateState[ChatBroadcastRequest](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
  }
}
