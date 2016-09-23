package improbable.natures.chat

import improbable.behaviours.chat.{ChatBroadcastListenerHandlerBehaviour, ChatBroadcastRequestHandlerBehaviour}
import improbable.behaviours.delegates.DelegateChatBroadcastRequestToClient
import improbable.chat.{ChatBroadcastListener, ChatBroadcastRequest}
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor

object ChatNature extends NatureDescription{
  override def dependencies: Set[NatureDescription] = Set.empty;
  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set(
    descriptorOf[ChatBroadcastRequestHandlerBehaviour],
    descriptorOf[ChatBroadcastListenerHandlerBehaviour]
  )

  def apply() : NatureApplication = {
    application(
      states = Seq(
        ChatBroadcastListener(messageHistory = List.empty),
        ChatBroadcastRequest()
      )
    )
  }
}
