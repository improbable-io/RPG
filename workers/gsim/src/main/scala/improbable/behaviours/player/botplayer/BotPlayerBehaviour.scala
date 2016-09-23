package improbable.behaviours.player

import improbable.logging.Logger
import improbable.Cancellable
import improbable.checks.CheckClientHasControl
import improbable.math.Vector3f
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World
import improbable.player.PlayerMovementStateWriter

import scala.concurrent.duration._
import scala.util.Random

class BotPlayerBehaviour(playerEntity : Entity,
                         movementStateWriter: PlayerMovementStateWriter,
                         logger : Logger,
                         world : World) extends EntityBehaviour {

  var currentlyStarted: Boolean = false
  var movementCallbackLoop: Cancellable = null

  override def onReady(): Unit =
  {
    playerEntity.watch[CheckClientHasControl].bind.isDelegated {
      isDelegated =>
        if (isDelegated && currentlyStarted)
          stop()
        else if (!isDelegated && !currentlyStarted)
          start()
        currentlyStarted = !isDelegated
    }
  }

  def start(): Unit =
  {
    val distanceOffset = 8
    movementCallbackLoop = world.timing.every(3 second) {
      val currentTarget = new Vector3f(movementStateWriter.targetPosition.x + Random.nextInt(distanceOffset*2)-distanceOffset,
                                    0,
                                    movementStateWriter.targetPosition.y + Random.nextInt(distanceOffset*2)-distanceOffset)
      movementStateWriter.update.targetPosition(currentTarget).finishAndSend()
    }
  }

  def stop(): Unit =
  {
    movementCallbackLoop.cancel()
  }
}
