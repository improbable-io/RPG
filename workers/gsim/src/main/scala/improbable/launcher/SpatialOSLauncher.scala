package improbable.launcher

import improbable.dapi.{Launcher, LaunchConfig}

/**
 * Run this to start your Simulation!
 */
object SpatialOSLauncherWithManualWorkers extends SpatialOSLauncher(ManualWorkerStartup)
object SpatialOSLauncherWithAutomaticWorkers extends SpatialOSLauncher(AutomaticWorkerStartup)

class SpatialOSLauncher(launchConfig: LaunchConfig, args: String*) extends App {
  val allOptions = Seq(
    "--entity_activator=improbable.corelib.entity.CoreLibraryEntityActivator",
    "--resource_based_config_name=one-gsim-one-jvm"
  ) ++ args
  Launcher.startGame(launchConfig, allOptions: _*)
}