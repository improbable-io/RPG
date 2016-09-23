package improbable.behaviours.delegates

import improbable.logging.Logger
import improbable.Cancellable
import improbable.chat.ChatBroadcastRequest
import improbable.checks.{CheckClientHasControlWriter, CheckIsClientSide}
import improbable.corelib.util.{EntityOwnerUtils, EntityOwnerWriter}
import improbable.papi.engine.EngineId
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.player._
import improbable.unity.papi.SpecificEngineConstraint

import scala.collection.mutable.ListBuffer

case class EntityOwnerRequestMsg( ownerId : EngineId ) extends CustomMsg
case class EntityOwnerRemoveMsg() extends CustomMsg

class ClientDelegationManager(entity : Entity,
                              entityOwnerWriter: EntityOwnerWriter,
                              checkClientHasControlWriter: CheckClientHasControlWriter,
                              world: World,
                              logger: Logger) extends EntityBehaviour {

  val delegates : ListBuffer[Cancellable]  = ListBuffer.empty;
  override def onReady(): Unit = {
    world.messaging.onReceive
    {
      case EntityOwnerRequestMsg(ownerId: EngineId) =>
        entityOwnerWriter.update.ownerId(Option(ownerId.toString())).finishAndSend()
        logger.info(s"Attempted to update owner id to $ownerId")
        delegateToClient(ownerId)
      case EntityOwnerRemoveMsg() =>
        logger.info(s"Attempted to remove all delegates")
        removeDelegationsFromClient()
    }
  }

  def delegateToClient(clientId: EngineId) {
    if (!clientId.isEmpty) {
      // Set flag first halt AI behaviours and avoid write errors when states are delegated
      checkClientHasControlWriter.update.isDelegated(true).finishAndSend()
      logger.info("ClientDelegationManager clientId not empty - delegating to client")
      delegates += entity.delegateState[CheckIsClientSide](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
      delegates += entity.delegateState[PlayerAnimation](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
      delegates += entity.delegateState[PlayerAppearanceState](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
      delegates += entity.delegateState[PlayerMovementState](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
      delegates += entity.delegateState[PlayerActionRequest](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
      delegates += entity.delegateState[PlayerTransactionRequest](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
      delegates += entity.delegateState[ChatBroadcastRequest](SpecificEngineConstraint(EntityOwnerUtils.ownerIdOf(entity)))
    }
    else {
      logger.error("ClientDelegationManager clientId is empty - could not delegate")
    }
  }

  def removeDelegationsFromClient(): Unit =
  {
    for ( delegate <- delegates) {
      delegate.cancel()
    }
    delegates.clear()
    // Un-delegate first before initiating AI behaviours to avoid errors writing to un-delegated states
    checkClientHasControlWriter.update.isDelegated(false).finishAndSend()
  }
}
