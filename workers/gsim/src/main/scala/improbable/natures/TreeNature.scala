package improbable.natures

import improbable.actions.{ActionAvailability, ActionTypeEnum, ActionTypeEnumWrapper}
import improbable.behaviours.supply.SupplyActionRequestHandlerBehaviour
import improbable.checks.CheckIsInteractive
import improbable.corelib.natures.{BaseNature, NatureApplication, NatureDescription}
import improbable.corelibrary.transforms.TransformNature
import improbable.math.Vector3d
import improbable.papi.entity.EntityPrefab
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.supplies.{Supply, SupplyTypeEnum}

object TreeNature extends NatureDescription{
  override def dependencies: Set[NatureDescription] = Set[NatureDescription](
    BaseNature,
    TransformNature
  )
  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set(
    descriptorOf[SupplyActionRequestHandlerBehaviour]
  )

  def apply(position: Vector3d) : NatureApplication = {
    application(
      states = Seq(
        Supply(
          isDepletable = true,
          currentAmount = 50,
          maxCapacity = 50,
          supplyType = SupplyTypeEnum.WOOD,
          harvestDuration = 0.5f,
          harvestYield = 2
          ),
        CheckIsInteractive(),
        ActionAvailability(List(
            ActionTypeEnumWrapper(ActionTypeEnum.HARVEST),
            ActionTypeEnumWrapper(ActionTypeEnum.TALK)
        ))
      ),
      natures = Seq(
        BaseNature(entityPrefab = EntityPrefab("TreePrefab"), isPhysical=true),
        TransformNature(position)
      )
    )
  }
}
