using System.Collections.Generic;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.ResponseTypes
{
    public class PreCheckResult
    {
        public bool IsReady { get; set; }
        public List<Area> IncompleteAreas { get; set; }

        PreCheckResult(bool isReady, List<Area> incompleteAreas)
        {
            IsReady = isReady;
            IncompleteAreas = incompleteAreas;
        }

        public static PreCheckResult CreateResult(bool isReady, List<Area> incompleteAreas)
        {
            return new PreCheckResult(isReady, incompleteAreas);
        }
    }
}