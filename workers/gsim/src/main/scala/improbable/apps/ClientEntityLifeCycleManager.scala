package improbable.apps

import improbable.logging.Logger
import improbable.natures.PlayerNature
import improbable.papi.EntityId
import improbable.papi.engine.EngineId
import improbable.papi.world.AppWorld
import improbable.papi.world.messaging.{EngineConnected, EngineDisconnected}
import improbable.papi.worldapp.WorldApp
import spray.json._
import DefaultJsonProtocol._
import improbable.behaviours.delegates.{EntityOwnerRemoveMsg, EntityOwnerRequestMsg}
import improbable.playfab.PlayFabManager
import improbable.unity.fabric.engine.EnginePlatform


class ClientEntityLifeCycleManager(appWorld : AppWorld, logger : Logger) extends WorldApp {
  private var clientIdToEntityIdMap = Map[EngineId, EntityId]()
  private var userIdToEntityIdMap = Map[String, EntityId]()

  logger.info("world app started")

  def connectPlayer(clientId: EngineId, userId: String, playFabId : String = ""): Unit = {
    if (!userIdToEntityIdMap.contains(userId))
    {
      createPlayer(clientId, userId, playFabId)
    }
    delegatePlayerToClient(clientId, userId, userIdToEntityIdMap(userId))
  }

  def createPlayer(clientId: EngineId, userId: String, playFabId : String): Unit =  {
    val playerEntityId = appWorld.entities.spawnEntity(PlayerNature(clientId))
    userIdToEntityIdMap += userId -> playerEntityId
    PlayFabManager.clientIdToPlayFabIdMap += clientId -> playFabId
    logger.warn(s"UserId=${userId} with clientId=${clientId} connection created Player entityId=${playerEntityId} and playFabId=${playFabId}")
  }

  def delegatePlayerToClient(clientId: EngineId, userId: String, playerEntityId: EntityId): Unit = {
    clientIdToEntityIdMap += clientId -> playerEntityId
    appWorld.messaging.sendToEntity(playerEntityId, EntityOwnerRequestMsg(clientId))
    logger.warn(s"UserId=${userId} with clientId=${clientId} was delegated control of entityId=${playerEntityId}")
  }

  def disconnectPlayer(clientId: EngineId): Unit = {
    clientIdToEntityIdMap.get(clientId) match {
      case Some(entityId) =>
        appWorld.messaging.sendToEntity(clientIdToEntityIdMap(clientId), EntityOwnerRemoveMsg())
        //TODO: Prompt entity to perform AI behaviours? Or leave that to the delegation manager?
        clientIdToEntityIdMap -= clientId

        logger.info(s"Client $clientId has disconnect. Unmapping from entity $entityId")
      case None =>
        logger.warn(s"User disconnected but could not find entity id for player: $clientId")
    }
  }

  def onWorkerConnected(engineConnectedMessage: EngineConnected): Unit = {
    engineConnectedMessage match {
      case EngineConnected(clientId, EnginePlatform.UNITY_CLIENT_ENGINE, metadata) =>
        val metadataMap: Map[String, String] = metadata.parseJson.convertTo[Map[String, String]]
        connectPlayer(clientId, metadataMap("userId"), metadataMap("playFabId"))
      case _ =>
    }
  }

  def onWorkerDisconnected(engineDisconnectedMsg: EngineDisconnected): Unit = {
    engineDisconnectedMsg match {
      case EngineDisconnected(clientId, EnginePlatform.UNITY_CLIENT_ENGINE) =>
        disconnectPlayer(clientId)
      case _ =>
    }
  }

  appWorld.messaging.subscribe {
    case engineConnectedMsg: EngineConnected =>
      logger.info("pre onWorkerConnected")
      onWorkerConnected(engineConnectedMsg)
      logger.info("post onWorkerConnected")
    case engineDisconnectedMsg: EngineDisconnected =>
      onWorkerDisconnected(engineDisconnectedMsg)
    case _ => logger.info("shouldn't come here")
  }


}
