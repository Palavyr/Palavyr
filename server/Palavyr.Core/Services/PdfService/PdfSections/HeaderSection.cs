using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Services.PdfService.PdfSections
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


            builder.Append(
                @"<table style='table-layout:auto; border-collapse: collapse; text-align: right; float: right;'><tbody>");
            foreach (var item in items)
            {
                var key = item.Keys.ToList()[0];
                var value = item.Values.ToList()[0];
                builder.Append(@$"<tr><{key} style='padding: 2mm' scope='col'>{value}</{key}></tr>");
            }

            builder.Append(@"</tbody></table>");
            return builder.ToString();
        }


        private static string CreateHeaderSection(
            string imageUri,
            List<string> companyTable
        )
        {
            var builder = new StringBuilder();

            var firstTable = companyTable.Select(el => new Dictionary<string, string>() {["th"] = el}).ToList();
            firstTable.Add(new Dictionary<string, string>() {["th"] = DateTime.Now.ToString()});

            builder.Append(@"<section id='HEADER' style='display: flex; flex-direction: row; padding-top: .5in; padding-left: .5in; padding-right: .5in;'>");
            if (!string.IsNullOrWhiteSpace(imageUri) && !string.IsNullOrEmpty(imageUri))
            {
                builder.Append($@"<img src='{imageUri}' style='margin-left: 2rem; width: 200px; height: 200px; object-fit: contain;' />");
            }
            else
            {
                // builder.Append("<img style='margin-left: 2rem; width: 200px; height: 200px; />");
            }

            builder.Append(SingleColTableGenerator(firstTable));

            builder.Append("</section>");

            return builder.ToString();
        }

        public static string GetHeader(Account account, ILinkCreator linkCreator, string userDataBucket, string emailAddress)
        {
            var imageLocation = account.AccountLogoUri ?? "";
            var logoUri = "";
            if (!string.IsNullOrWhiteSpace(imageLocation))
            {
                logoUri = linkCreator.GenericCreatePreSignedUrl(imageLocation, userDataBucket);
            }

            var companyDetails = new List<string>()
            {
                $"<h2 style='padding-bottom: -20px;'>{account.CompanyName}</h2><h4 style='margin-top: -4rem;' >{account.PhoneNumber}</h4>", " ", emailAddress
            };

            var header = CreateHeaderSection(
                logoUri,
                companyDetails
            );
            return header;
        }
    }
}