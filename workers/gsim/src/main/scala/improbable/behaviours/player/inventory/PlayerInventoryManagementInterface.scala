package improbable.behaviours.player

import improbable.papi._
import improbable.papi.entity.behaviour.EntityBehaviourInterface
import improbable.supplies.SupplyTypeEnum._

trait PlayerInventoryManagementInterface extends EntityBehaviourInterface {
  def handleSupplyTransaction(supplyLossType: SupplyTypeEnum, supplyLossAmount: Int, supplyGainType: SupplyTypeEnum, supplyGainAmount: Int)
}