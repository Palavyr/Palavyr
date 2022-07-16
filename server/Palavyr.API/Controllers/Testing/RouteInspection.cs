using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Palavyr.API.Controllers.Testing
{
    public class RouteInspection : PalavyrBaseController
    {
        private readonly IEnumerable<EndpointDataSource> endpointSources;

        public RouteInspection(IEnumerable<EndpointDataSource> endpointSources)
        {
            this.endpointSources = endpointSources;
        }

        [HttpGet("palavyr-routes")]
        public void GetRoutes()
        {
            var path = @"C:\Users\paule\code\temp\the-all-route.txt";
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path);
            }

            var endpoints = endpointSources
                .SelectMany(es => es.Endpoints)
                .OfType<RouteEndpoint>()
                .ToList();

            using var writer = new StreamWriter(path, true);
            foreach (var endpoint in endpoints)
            {
                writer.WriteLine(endpoint.RoutePattern.RawText.Replace("api/", ""));
            }
        }
    }
}