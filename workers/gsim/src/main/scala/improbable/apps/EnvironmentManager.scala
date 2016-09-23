package improbable.apps

import improbable.logging.Logger
import improbable.math.Vector3d
import improbable.natures.TreeNature
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.WorldApp

class EnvironmentManager(world: AppWorld, logger: Logger) extends WorldApp {
  logger.info("world app started")

  for (i <- 1 to 50)
    world.entities.spawnEntity(TreeNature(
      Vector3d(
        math.random*25,
        0,
        math.random*25L)
      )
    )
}