using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class PreCheckResultResource
    {
        public bool IsReady { get; set; }
        public List<PreCheckErrorResource> PreCheckErrors { get; set; }
        public bool ApiKeyExists { get; set; }

        private PreCheckResultResource(bool isReady, List<PreCheckErrorResource> errors)
        {
            IsReady = isReady;
            PreCheckErrors = errors;
        }

        private PreCheckResultResource(bool apiKeyExists)
        {
            ApiKeyExists = apiKeyExists;
        }

        public static PreCheckResultResource CreateConvoResult(bool isReady, List<PreCheckErrorResource> errors)
        {
            return new PreCheckResultResource(isReady, errors);
        }

        public static PreCheckResultResource CreateApiKeyResult(bool apiKeyExists)
        {
            return new PreCheckResultResource(apiKeyExists);
        }
    }
}