using System;
using System.Collections.Generic;
using Server.Domain;
using Server.Domain.App.schema;

namespace Palavyr.API.Controllers
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
                            Fee = new StaticFee() {FeeId = Guid.NewGuid().ToString(), Min = 50.00, Max = 200.00},
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
                            Fee = new StaticFee() {FeeId = Guid.NewGuid().ToString(), Min = 175.00, Max = 175.00},
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
                            Fee = new StaticFee() {FeeId = Guid.NewGuid().ToString(), Min = 330.00, Max = 0.00},
                            Range = false,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = areaIdentifier,
                            AccountId = accountId
                        }
                    }
                },
                new StaticTablesMeta()
                {
                    AreaIdentifier = areaIdentifier,
                    AccountId = accountId,
                    Description = "Your purchase of a Cavvy comes with a year supply of health checks and diet maintenance!",
                    TableOrder = 1,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Amazingly tasty dog treats!",
                            Fee = new StaticFee() {FeeId = Guid.NewGuid().ToString(), Min = 250.00, Max = 0.00},
                            Range = false,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = areaIdentifier,
                            AccountId = accountId
                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Health Checks",
                            Fee = new StaticFee() {FeeId = Guid.NewGuid().ToString(), Min = 350.00, Max = 0.00},
                            Range = false,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = areaIdentifier,
                            AccountId = accountId
                        },
                        new StaticTableRow()
                        {
                            RowOrder = 2,
                            Description = "Flea Drops",
                            Fee = new StaticFee() {FeeId = Guid.NewGuid().ToString(), Min = 75.00, Max = 0.00},
                            Range = false,
                            PerPerson = true,
                            TableOrder = 1,
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
            string groupId,
            string emailTemplate
            )
        {

            var areaMetas = CreateDefaultStaticTableMetas(areaIdentifier, accountId);
            var area = new Area()
            {
                AreaIdentifier = areaIdentifier,
                AccountId = accountId,
                AreaName = areaName,
                AreaDisplayTitle = "HouseKeeping",
                Epilogue = "The following is a ballpark itemized estimate of what you can expect to pay when purchasing a cavvy from us. These tables are asjustable given the specific details of your request and you are free to remove items from this estimate when making your purchase.",
                Prologue = "Note: These values are a rough estimate only! The final cost of your purchase may vary depending on your final purchase decision.",
                EmailTemplate = emailTemplate,
                ConversationNodes = conversationNodes,
                StaticTablesMetas = areaMetas,
                DynamicTableMetas = dynamicTableMetas,
                GroupId = groupId
            };
            return area;
        }
    }
}