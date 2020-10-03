using System.Collections.Generic;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.ResponseTypes
{
    public class PreCheckResult
    {
        public bool IsReady { get; set; }
        public List<Area> IncompleteAreas { get; set; }
        public bool ApiKeyExists { get; set; }

        PreCheckResult(bool isReady, List<Area> incompleteAreas)
        {
            IsReady = isReady;
            IncompleteAreas = incompleteAreas;            
        }
        PreCheckResult(bool apiKeyExists)
        {

            ApiKeyExists = apiKeyExists;
        }

        public static PreCheckResult CreateConvoResult(List<Area> incompleteAreas, bool isReady)
        {
            return new PreCheckResult(isReady: isReady, incompleteAreas: incompleteAreas);
        }

        public static PreCheckResult CreateApiKeyResult(bool apiKeyExists)
        {
            return new PreCheckResult(apiKeyExists: apiKeyExists);
        }
    }
    
}