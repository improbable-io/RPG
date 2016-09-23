package improbable.playfab

import java.util

import scala.collection.JavaConversions._
import com.playfab.PlayFabErrors.{PlayFabError, PlayFabErrorCode}
import com.playfab.PlayFabServerModels.GrantItemsToUserRequest
import com.playfab.{PlayFabServerAPI, PlayFabSettings}
import improbable.logging.Logger
import improbable.papi.engine.EngineId

object PlayFabManager {

  private val catalog = "prototype"

  // For more info, see https://api.playfab.com/Documentation/Server
  PlayFabSettings.DeveloperSecretKey = "insert_your_secret_key_here"
  PlayFabSettings.TitleId = "insert_title_id_here"

  var clientIdToPlayFabIdMap : Map[EngineId, String] = Map.empty

  private def checkError(error: PlayFabError, logger: Logger): Boolean = {
    if (error != null) {
      if (error.pfErrorCode != PlayFabErrorCode.Success) {
        // Display error
        logger.error(s"${error.pfErrorCode.toString} message: ${error.errorMessage}")
        if (error.errorDetails != null) {
          for (s <- error.errorDetails) {
            var errorDetail = s._1 + ":"
            for (sd <- s._2) {
              errorDetail += " " + sd
            }
            logger.error(errorDetail)
          }
        }

        return false
      }
    }
    return true
  }

  def grantItem(playFabId: String, itemId: String, count: Int = 1, logger: Logger) : Boolean = {
    logger.info(s"grantItem(), playFabId=${playFabId}")
    val req = new GrantItemsToUserRequest()
    req.CatalogVersion = catalog
    req.PlayFabId = playFabId
    req.ItemIds = new util.ArrayList[String](count)
    for (_ <- 1 to count)
      req.ItemIds.add(itemId)

    val result = PlayFabServerAPI.GrantItemsToUser(req)
    return checkError(result.Error, logger)
  }

}
