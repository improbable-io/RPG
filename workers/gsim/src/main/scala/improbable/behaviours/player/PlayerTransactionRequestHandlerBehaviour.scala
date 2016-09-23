package improbable.behaviours.player

import improbable.logging.Logger
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.player.{PlayerTransactionRequest, PlayerTransactionRequestPayload, PlayerInventory}
import improbable.supplies.SupplyTypeEnum
import improbable.supplies.SupplyTypeEnum.SupplyTypeEnum
import scala.collection.mutable.Map

class PlayerTransactionRequestHandlerBehaviour(playerEntity: Entity,
                                               playerInventoryManagementInterface: PlayerInventoryManagementInterface,
                                               logger: Logger,
                                               world: World) extends EntityBehaviour {

  // Hardcode map for conversion rates - mapping from/to types to minimum from/to quantities
  var SUPPLY_CONVERSION_RATE_MAP = Map[(SupplyTypeEnum.Value, SupplyTypeEnum.Value), (Int, Int)]()
  SUPPLY_CONVERSION_RATE_MAP((SupplyTypeEnum.WOOD, SupplyTypeEnum.COINS)) = (1, 2)
  SUPPLY_CONVERSION_RATE_MAP((SupplyTypeEnum.COINS, SupplyTypeEnum.WOOD)) = (2, 1)
  SUPPLY_CONVERSION_RATE_MAP((SupplyTypeEnum.COPPER, SupplyTypeEnum.COINS)) = (1, 5)
  SUPPLY_CONVERSION_RATE_MAP((SupplyTypeEnum.COINS, SupplyTypeEnum.COPPER)) = (5, 1)
  SUPPLY_CONVERSION_RATE_MAP((SupplyTypeEnum.TIN, SupplyTypeEnum.COINS)) = (1, 10)
  SUPPLY_CONVERSION_RATE_MAP((SupplyTypeEnum.COINS, SupplyTypeEnum.TIN)) = (10, 1)

  override def onReady(): Unit = {
    playerEntity.watch[PlayerTransactionRequest].onTransactionRequest {
      case PlayerTransactionRequestPayload(fromSupplyType: SupplyTypeEnum, toSupplyType: SupplyTypeEnum) =>
        handlePlayerTransactionRequest(fromSupplyType, toSupplyType)
    }
  }

  def handlePlayerTransactionRequest(fromSupplyType: SupplyTypeEnum, toSupplyType: SupplyTypeEnum): Unit = {
    val (quantityOfTypeLost: Int, quantityOfTypeGained: Int) = SUPPLY_CONVERSION_RATE_MAP.getOrElse((fromSupplyType, toSupplyType), (0,0))
    if (quantityOfTypeGained == 0) {
      logger.info(s"handlePlayerTransactionRequest() has no valid conversion rate for $fromSupplyType to $toSupplyType")
    }
    else {
        playerInventoryManagementInterface.handleSupplyTransaction(supplyLossType = fromSupplyType,
                                                                    supplyLossAmount = quantityOfTypeLost,
                                                                    supplyGainType = toSupplyType,
                                                                    supplyGainAmount = quantityOfTypeGained)
    }
  }
}
