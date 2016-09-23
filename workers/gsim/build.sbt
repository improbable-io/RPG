lazy val rootProject = SpatialOSBuild.root
  .settings(libraryDependencies += "improbable" %% "corelibrary" % SpatialOSBuild.improbableVersion)
  .settings(libraryDependencies += "io.spray" %%  "spray-json" % "1.3.2")
