package improbable.natures

import improbable.behaviours.delegates._
import improbable.behaviours.player.inventory.PlayerInventoryManagementBehaviour
import improbable.behaviours.player.{BotPlayerBehaviour, PlayerActionRequestHandlerBehaviour, PlayerTransactionRequestHandlerBehaviour}
import improbable.checks.{CheckClientHasControl, CheckIsClientSide}
import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelib.util.EntityOwner
import improbable.corelibrary.transforms.TransformNature
import improbable.math.Vector3f
import improbable.natures.chat.ChatNature
import improbable.papi.engine.EngineId
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.player._

object PlayerNature extends NatureDescription{
  override def dependencies: Set[NatureDescription] = Set[NatureDescription](
    BaseNature,
    TransformNature,
    ChatNature
  )

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set(
    descriptorOf[ClientDelegationManager],
    // inventory management
    descriptorOf[PlayerInventoryManagementBehaviour],
    descriptorOf[PlayerTransactionRequestHandlerBehaviour],
    // interaction management
    descriptorOf[PlayerActionRequestHandlerBehaviour],
    // AI behaviour
    descriptorOf[BotPlayerBehaviour]
  )

  def apply(clientId : EngineId) : NatureApplication = {
    application(
      states = Seq(
        EntityOwner(Some(clientId)),
        CheckIsClientSide(),
        PlayerAppearanceState(color = Vector3f(0, 0, 1.0f)), // blue color default for player
        PlayerMovementState(Vector3f.zero),
        PlayerInventory(Map.empty),
        PlayerActionRequest(),
        PlayerTransactionRequest(),
        PlayerAnimation(),
        CheckClientHasControl(false)
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab("PlayerPrefab"), isPhysical=true),
        TransformNature(),
        ChatNature()
      )
    )
  }
}
