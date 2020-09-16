﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DashboardServer.API.chatUtils;
using Server.Domain.AccountDB;

namespace PDFService
{
    public static class HeaderSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"> A dictionary of row style (th or tr) and item value</param>
        /// <returns></returns>
        private static string SingleColTableGenerator(List<Dictionary<string, string>> items)
        {
            var builder = new StringBuilder();

            builder.Append(@"<table style='table-layout:auto; width: 100%; border-collapse: collapse; text-align: left;'><tbody>");
            foreach (var item in items)
            {
                var key = item.Keys.ToList()[0];
                var value = item.Values.ToList()[0];
                builder.Append(@$"<tr><{key} style='padding: 2mm' scope='col'>{value}</{key}></tr>");
            }
            builder.Append(@"</tbody></table>");
            return builder.ToString();
        }

        private static string TwoColTableGenerator(List<Dictionary<string, string>> items)
        {
            var builder = new StringBuilder();

            builder.Append($@"<table style='table-layout:auto; width: 100%; border-collapse:collapse; text-align: left'><tbody>");

            foreach (var item in items)
            {
                var key = item.Keys.ToList()[0];
                var value = item.Values.ToList()[0];
                builder.Append(@$"<tr><th style='padding: 2mm' scope='col'>{key}:</th><td>{value}</td></tr>");
            };
            builder.Append(@"</tbody></table>");
            return builder.ToString();
        }

        private static string CreateHeaderSection(string imageLocation, List<string> companyTable,
            List<Dictionary<string, string>> criticalTable)
        {
            var builder = new StringBuilder();

            var firstTable = companyTable.Select(el => new Dictionary<string, string>() {["th"] = el}).ToList();
            firstTable.Add(new Dictionary<string, string>() {["th"] = DateTime.Today.ToString()});
            
            builder.Append(@"<section id='HEADER' style='display: inline-block'>");
            builder.Append(@"<div style='height: 100%; float: left; text-align: center; margin-right: 10mm'>");
            builder.Append(
                $@"<img src='{imageLocation}' style='max-width: 250px; max-height: 250px; vertical-align: middle;'>");
            builder.Append($@"</div>");
            builder.Append($@"<div style='max-height: 100%; float: right;'>");
            builder.Append(SingleColTableGenerator(firstTable));
            builder.Append(TwoColTableGenerator(criticalTable));
            builder.Append($@"</div>");
            builder.Append("</section>");

            return builder.ToString();
        }
        
        public static string GetHeader(UserAccount userAccount, CriticalResponses response)
        {
            var imageLocation = userAccount.AccountLogoUri ?? "";
            var companyDetails = new List<string>()
            {
                 "<h2>" + userAccount.CompanyName + "</h2>", userAccount.PhoneNumber, userAccount.EmailAddress
            };

            var header = HeaderSection.CreateHeaderSection(userAccount.AccountLogoUri, companyDetails, response.CreateResponse());
            return header;
        }
    }
}