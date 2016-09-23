package improbable.behaviours.player

import improbable.logging.Logger
import improbable.actions.ActionTypeEnum
import improbable.actions.ActionTypeEnum.ActionTypeEnum
import improbable.behaviours.supply.SupplyActionRequestMsg
import improbable.papi.EntityId
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.player.{PlayerActionRequest, PlayerActionRequestPayload}

class PlayerActionRequestHandlerBehaviour(playerEntity : Entity, logger : Logger, world : World) extends EntityBehaviour {

  override def onReady(): Unit = {
    playerEntity.watch[PlayerActionRequest].onActionRequest{
      case PlayerActionRequestPayload(targetEntityId: EntityId, action: ActionTypeEnum) =>
        logger.info(s"Player, entityId=${playerEntity.entityId}, action=${action}, targetId=${targetEntityId}")

        handlePlayerActionRequest(action, playerEntity, targetEntityId)
    }
  }

  def handlePlayerActionRequest(action : ActionTypeEnum.Value, player : Entity, targetEntityId : EntityId) : Unit = {
    logger.info("handlePlayerActionRequest()")
    world.messaging.sendToEntity(targetEntityId, SupplyActionRequestMsg(actionType = action, playerEntityId = player.entityId))
  }
}
