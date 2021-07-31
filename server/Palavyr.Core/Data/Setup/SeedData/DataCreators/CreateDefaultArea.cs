﻿using System;
using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Data.Setup.SeedData.DataCreators
{
    public static class CreateDefaultArea
    {
        private static List<StaticTablesMeta> CreateDefaultStaticTableMetas(string areaIdentifier, string accountId)
        {
            var staticTableMetas = new List<StaticTablesMeta>()
            {
                new StaticTablesMeta()
                {
                    AreaIdentifier = areaIdentifier,
                    AccountId = accountId,
                    Description = "We typically provide service packages. Our basic package for dogs includes several other services, such as 6 months of dog walks and grooming.",
                    TableOrder = 0,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Dog Baths",
                            Fee = StaticFee.CreateNew(feeId: Guid.NewGuid().ToString(), min: 50.00, max: 200.00, accountId: accountId, areaIdentifier: areaIdentifier),
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = areaIdentifier,
                            AccountId = accountId
                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Nail Clipping",
                            Fee = StaticFee.CreateNew(feeId: Guid.NewGuid().ToString(), min: 175.00, max: 175.00, accountId: accountId, areaIdentifier: areaIdentifier),
                            Range = false,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = areaIdentifier,
                            AccountId = accountId
                        },
                        new StaticTableRow()
                        {
                            RowOrder = 2,
                            Description = "Doggy Training",
                            Fee = StaticFee.CreateNew(feeId: Guid.NewGuid().ToString(), min: 350.00, max: 0.00, accountId: accountId, areaIdentifier: areaIdentifier),
                            Range = false,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = areaIdentifier,
                            AccountId = accountId
                        }
                    }
                }
            };
            return staticTableMetas;
        }

        public static Area CreateDefault(
            string areaIdentifier,
            string accountId,
            string areaName,
            List<ConversationNode> conversationNodes,
            List<DynamicTableMeta> dynamicTableMetas,
            string emailTemplate,
            string defaultEmail
        )
        {
            var areaMetas = CreateDefaultStaticTableMetas(areaIdentifier, accountId);
            var area = new Area()
            {
                AreaIdentifier = areaIdentifier,
                AccountId = accountId,
                AreaName = areaName,
                AreaDisplayTitle = "Purchase a Cavvy",
                Epilogue = "The following is a ballpark itemized estimate of what you can expect to pay when purchasing a cavvy from us. These tables are asjustable given the specific details of your request and you are free to remove items from this estimate when making your purchase.",
                Prologue = "Note: These values are a rough estimate only! The final cost of your purchase may vary depending on your final purchase decision.",
                EmailTemplate = emailTemplate,
                ConversationNodes = conversationNodes,
                StaticTablesMetas = areaMetas,
                DynamicTableMetas = dynamicTableMetas,
                AreaSpecificEmail = defaultEmail,
                EmailIsVerified = false,
                Subject = "Thanks for contacting us!",
                FallbackSubject = "",
                FallbackEmailTemplate = "",
                SendPdfResponse = true
            };
            return area;
        }
    }
}