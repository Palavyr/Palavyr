using System.Collections.Generic;

namespace PDFService.PdfSections.Util
{
    public class CriticalResponses
    {
        private List<Dictionary<string, string>> CriticalValues { get; set; }

        public CriticalResponses(List<Dictionary<string, string>> criticalValues)
        {
            CriticalValues = criticalValues;
        }

        public List<Dictionary<string, string>> CreateResponse()
        {
            var response = new List<Dictionary<string, string>>();

            foreach (var keyValDict in CriticalValues)
            {
                foreach (var (key,val) in keyValDict)
                {
                    response.Add(
                        new Dictionary<string, string>()
                        {
                            {key, val}
                        });                
                }
            }
            
            return response;
        }
    }
}