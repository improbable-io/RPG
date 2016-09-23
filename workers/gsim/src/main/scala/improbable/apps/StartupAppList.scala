package improbable.apps;

object StartupAppList {
  val appsToStart = List(
    classOf[ClientEntityLifeCycleManager],
    classOf[EnvironmentManager],
    classOf[ChatManager]
  )
}