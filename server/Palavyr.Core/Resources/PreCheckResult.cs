using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class PreCheckError
    {
        public string AreaName { get; set; }
        public List<string> Reasons { get; } = new List<string>();
    }

    public class PreCheckResult
    {
        public bool IsReady { get; set; }
        public List<PreCheckError> PreCheckErrors { get; set; }
        public bool ApiKeyExists { get; set; }

        private PreCheckResult(bool isReady, List<PreCheckError> errors)
        {
            IsReady = isReady;
            PreCheckErrors = errors;
        }

        private PreCheckResult(bool apiKeyExists)
        {
            ApiKeyExists = apiKeyExists;
        }

        public static PreCheckResult CreateConvoResult(bool isReady, List<PreCheckError> errors)
        {
            return new PreCheckResult(isReady, errors);
        }

        public static PreCheckResult CreateApiKeyResult(bool apiKeyExists)
        {
            return new PreCheckResult(apiKeyExists);
        }
    }
}