﻿using System.Collections.Generic;

namespace DashboardServer.API.chatUtils
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
            
            // for (var i = 0; i < CriticalValues.Count; i++)
            // {
            //     var val = CriticalValues[i];
            //     response.Add(
            //         new Dictionary<string, string>()
            //         {
            //             {$"Key Info #{i.ToString()}", val}
            //         });
            // }
            return response;
        }
    }
}