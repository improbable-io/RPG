package improbable.behaviours.player.inventory

import improbable.logging.Logger
import improbable.behaviours.player.PlayerInventoryManagementInterface
import improbable.corelib.util.EntityOwner
import improbable.papi.EntityId
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.player.PlayerInventoryWriter
import improbable.playfab.PlayFabManager
import improbable.supplies.SupplyTypeEnum.SupplyTypeEnum

// Note: supplyGiftAmount should be positive (for now...)
// Increases supply of a given type with no corresponding cost
case class SupplyGiftMsg(supplyEntityId : EntityId, supplyType : SupplyTypeEnum, supplyTransferAmount : Int) extends CustomMsg {}

class PlayerInventoryManagementBehaviour(playerEntity : Entity, playerInventoryWriter : PlayerInventoryWriter, world : World, logger : Logger) extends EntityBehaviour with PlayerInventoryManagementInterface {
  var engineId: String = ""

  engineId = playerEntity.watch[EntityOwner].ownerId match {
    case Some(Some(id)) => id
    case Some(None) => ""
    case None => ""
  }

  override def onReady(): Unit = {
    // TODO: Download initial state
    world.messaging.onReceive{
      case SupplyGiftMsg(supplyEntityId, supplyType, supplyTransferAmount) =>
        handleSupplyGift(supplyEntityId, supplyType, supplyTransferAmount)
    }
  }

  def handleSupplyGift(supplyEntityId: EntityId, supplyType: SupplyTypeEnum, supplyTransferAmount: Int): Unit = {
    //if (PlayFabManager.grantItem(engineId, supplyType.toString, supplyTransferAmount, logger)) {
      // Calculate the update
      val currentSupplyAmount = playerInventoryWriter.supplyQuantityMap.getOrElse(supplyType, 0)
      val postTransferAmount = currentSupplyAmount + supplyTransferAmount
      // Perform the update
      val quantityMap = playerInventoryWriter.supplyQuantityMap.updated(supplyType, postTransferAmount)
      playerInventoryWriter.update.supplyQuantityMap(quantityMap).finishAndSend()
      logger.info(s"Successful transfer of supply: ${supplyType} from ${currentSupplyAmount} -> ${postTransferAmount} (delta=${supplyTransferAmount}")

      if (!PlayFabManager.grantItem(PlayFabManager.clientIdToPlayFabIdMap(engineId), supplyType.toString, supplyTransferAmount, logger)) {
        logger.warn(s"Unable to grant item in Playfab")
      }
  }

  def handleSupplyTransaction(supplyLossType: SupplyTypeEnum, supplyLossAmount: Int, supplyGainType: SupplyTypeEnum, supplyGainAmount: Int): Unit =
  {
    // Calculate the update for the loss
    val currentSupplyAmountForLossType = playerInventoryWriter.supplyQuantityMap.getOrElse(supplyLossType, 0)
    val postLossAmount = currentSupplyAmountForLossType - supplyLossAmount
    // Check legality of transaction
    if (postLossAmount >= 0) {
      // Calculate the update for the gain
      val currentSupplyAmountForGainType = playerInventoryWriter.supplyQuantityMap.getOrElse(supplyGainType, 0)
      val postGainAmount = currentSupplyAmountForGainType + supplyGainAmount
      // Update map for loss and gain
      var quantityMap = playerInventoryWriter.supplyQuantityMap.updated(supplyLossType, postLossAmount).updated(supplyGainType, postGainAmount)
      // Write inventory map to state
      playerInventoryWriter.update.supplyQuantityMap(quantityMap).finishAndSend()
      logger.info(s"handleSupplyTransaction() successfully converted $supplyLossAmount $supplyLossType into $supplyGainAmount $supplyGainType")
    }
    else {
      logger.info(s"handleSupplyTransaction() cannot approve transaction due to lack of $supplyLossType ($currentSupplyAmountForLossType, need $supplyLossAmount)")
    }
  }
}
