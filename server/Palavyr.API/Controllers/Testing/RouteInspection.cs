using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;

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
            var endpoints = endpointSources
                .SelectMany(es => es.Endpoints)
                .OfType<RouteEndpoint>()
                .ToList();
            //
            // var path = @"C:\Users\paule\code\palavyr\RouteCheck\ApiRoutes.txt";
            // using var writer = new StreamWriter(path, true);
            // foreach (var endpoint in endpoints)
            // {
            //     writer.WriteLine(endpoint.RoutePattern.RawText.Replace("api/", ""));
            // }

            var returnTypesPath = @"C:\Users\paule\code\palavyr\RouteCheck\ReturnTypes.txt";
            using var returnWriter = new StreamWriter(returnTypesPath, true);
            var resourceTypes = new List<Type>();
            foreach (var endpoint in endpoints)
            {
                try
                {
                    var descriptor = endpoint.Metadata.ToList().SingleOrDefault(x => x is ControllerActionDescriptor);
                    if (descriptor is null)
                    {
                        Console.WriteLine($"Null Descriptor: {endpoint.RoutePattern.RawText}");
                    }

                    var typedDescriptor = descriptor as ControllerActionDescriptor;
                    if (typedDescriptor is null)
                    {
                        continue;
                    }


                    var returnType = typedDescriptor.MethodInfo.ReturnType;
                    if (returnType.IsGenericType) // Task<>
                    {
                        var outerArgs = returnType.GetGenericArguments();
                        foreach (var outerArg in outerArgs) // gonna be a resource type
                        {
                            if (outerArg.IsGenericType) // List<T>
                            {
                                var innerArgs = outerArg.GetGenericArguments();
                                foreach (var innerArg in innerArgs)
                                {
                                    if (resourceTypes.Contains(innerArg)) continue;
                                    WriteResourceType(innerArg, outerArg, endpoint);
                                    resourceTypes.Add(innerArg);
                                }
                            }
                            else
                            {
                                Console.WriteLine(outerArg.Name);
                                returnWriter.WriteLine(outerArg.Name);
                                if (!outerArg.IsPrimitive && outerArg.Name != "String")
                                {
                                    if (resourceTypes.Contains(outerArg)) continue;
                                    WriteResourceType(outerArg, null, endpoint);
                                    resourceTypes.Add(outerArg);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(returnType.Name);
                        returnWriter.WriteLine(returnType.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void WriteResourceType(Type resourceType, Type? outerArg, RouteEndpoint route)
        {
            var sb = new StringBuilder();
            
            var properties = resourceType.GetProperties();
            sb.AppendLine($"export type {resourceType.Name} = {{");

            if (resourceType.Name.StartsWith("ConversationRowsResource"))
            {
                ;
            }
            
            foreach (var propertyInfo in properties)
            {

                var propType = propertyInfo.PropertyType;
                if (propertyInfo.Name == "IsFixedSize")
                {
                    ;
                }
                if (propType.IsGenericType)
                {
                    // nullable!
                    var nullableArg = propType.GetGenericArguments();
                    if (nullableArg.Length > 1)
                    {
                        ;
                    }

                    if (outerArg != null)
                    {

                        var name = $"{propertyInfo.Name.FirstCharToLowerCase()}: {nullableArg[0].Name}[]";
                        sb.AppendLine(name);
                    }
                    else
                    {
                        var outerName = "";
                        sb.AppendLine($"{propertyInfo.Name.FirstCharToLowerCase()}: {outerName}{nullableArg[0].Name}");
                    }
                }
                else
                {
                    sb.AppendLine($"{propertyInfo.Name.FirstCharToLowerCase()}: {propertyInfo.PropertyType.Name}");
                }
            }

            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("");

            var resourceTypesPath = @"C:\Users\paule\code\palavyr\RouteCheck\ResourceTypes.txt";
            using var resourceWriter = new StreamWriter(resourceTypesPath, true);
            resourceWriter.Write(sb.ToString());
        }
    }

    public static class SnakeCaseExtensionMethods
    {
        public static string? FirstCharToLowerCase(this string? str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return str;
        }
    }
}