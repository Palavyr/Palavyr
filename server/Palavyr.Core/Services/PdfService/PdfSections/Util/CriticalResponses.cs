using System.Collections.Generic;

namespace Palavyr.Core.Services.PdfService.PdfSections.Util
{
    public interface ICriticalResponses
    {
        CriticalResponses Compile(List<Dictionary<string, string>> criticalValues);
        List<Dictionary<string, string>> CreateResponse();
    }

    public class CriticalResponses : ICriticalResponses
    {
        private List<Dictionary<string, string>> CriticalValues { get; set; }

        public CriticalResponses Compile(List<Dictionary<string, string>> criticalValues)
        {
            return new CriticalResponses
            {
                CriticalValues = criticalValues
            };
        }

        public List<Dictionary<string, string>> CreateResponse()
        {
            var response = new List<Dictionary<string, string>>();

            foreach (var keyValDict in CriticalValues)
            {
                foreach (var (key, val) in keyValDict)
                {
                    response.Add(
                        new Dictionary<string, string>
                        {
                            {key, val}
                        });
                }
            }

            return response;
        }
    }
}