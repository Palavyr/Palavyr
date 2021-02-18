using System.Collections.Generic;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.CommonResponseTypes
{
    public class PreCheckResult
    {
        public bool IsReady { get; set; }
        public List<Area> IncompleteAreas { get; set; }
        public bool ApiKeyExists { get; set; }

        private PreCheckResult(bool isReady, List<Area> incompleteAreas)
        {
            IsReady = isReady;
            IncompleteAreas = incompleteAreas;            
        }

        private PreCheckResult(bool apiKeyExists)
        {

            ApiKeyExists = apiKeyExists;
        }

        public static PreCheckResult CreateConvoResult(List<Area> incompleteAreas, bool isReady)
        {
            return new PreCheckResult(isReady, incompleteAreas);
        }

        public static PreCheckResult CreateApiKeyResult(bool apiKeyExists)
        {
            return new PreCheckResult(apiKeyExists);
        }
    }
    
}