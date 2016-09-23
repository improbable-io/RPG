package improbable.behaviours.supply

import improbable.logging.Logger
import improbable.actions.ActionTypeEnum
import improbable.actions.ActionTypeEnum.ActionTypeEnum
import improbable.behaviours.player.inventory.SupplyGiftMsg
import improbable.papi.EntityId
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.supplies.SupplyWriter

import scala.collection.mutable.Map

case class SupplyActionRequestMsg(actionType : ActionTypeEnum, playerEntityId : EntityId) extends CustomMsg

class SupplyActionRequestHandlerBehaviour(supplyEntity : Entity, supplyWriter : SupplyWriter, world : World, logger: Logger ) extends EntityBehaviour {

  val ACTION_HANDLER_MAP = Map[ActionTypeEnum, (EntityId) => Unit] ()
  ACTION_HANDLER_MAP(ActionTypeEnum.HARVEST) = handleHarvestActionRequest

  override def onReady(): Unit = {

    world.messaging.onReceive{
      case SupplyActionRequestMsg(actionType, playerEntityId) => {
        if (ACTION_HANDLER_MAP.contains(actionType)) {
          ACTION_HANDLER_MAP(actionType)(playerEntityId)
        }
        else
        {
          logger.info(s"Supply entity '${supplyEntity.entityId}' has no action '${actionType}' for player '${playerEntityId}' to perform")
        }
      }
    }
  }

  def handleHarvestActionRequest(playerEntityId : EntityId) : Unit = {

    logger.info("handle harvest action")

    if (supplyWriter.currentAmount > 0) {

      // Calculate available yield
      val availableYield = math.min(supplyWriter.currentAmount, supplyWriter.harvestYield)
      val postHarvestAmount = supplyWriter.currentAmount - availableYield

      logger.info(s"updating supply writer state from ${supplyWriter.currentAmount} -> ${postHarvestAmount}")

      // Update current amount based on yield to give
      supplyWriter.update.currentAmount(postHarvestAmount).finishAndSend()

      world.messaging.sendToEntity(playerEntityId, SupplyGiftMsg(
        supplyEntityId = supplyEntity.entityId,
        supplyType = supplyWriter.supplyType,
        supplyTransferAmount = availableYield))
    }
  }
}
