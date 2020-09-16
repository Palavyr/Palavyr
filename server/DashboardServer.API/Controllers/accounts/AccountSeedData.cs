using System;
using System.Collections.Generic;
using Server.Domain;

namespace DashboardServer.API.Controllers
{
    public class AccountSeedData
    {
        public List<Area> Areas { get; set; }
        public List<GroupMap> Groups { get; set; } = new List<GroupMap>();
        public AccountSeedData(string accountId)
        {
            var groupOneId = Guid.NewGuid().ToString();
            string areaTwoGroupId = null;
            
            Groups.Add(GroupMap.CreateGroupMap(groupOneId, null, "Group One", accountId)); ;
            var areaOneNodes = new List<ConversationNode>()
            {
                new ConversationNode()
                {
                    NodeId = "node-123",
                    AreaIdentifier = "abc-123",
                    Fallback = false,
                    Text = "Is toby a good boy?",
                    IsRoot = true,
                    NodeChildrenString = "node-456,node-789",
                    NodeType = "yesNo",
                    OptionPath = null,
                    AccountId = accountId
                },
                new ConversationNode()
                {
                    NodeId = "node-456",
                    AreaIdentifier = "abc-123",
                    Fallback = false,
                    Text = "Is toby a bad boy?",
                    IsRoot = false,
                    NodeChildrenString = "",
                    NodeType = "yesNo",
                    OptionPath = "Yes",
                    AccountId = accountId
                },
                new ConversationNode()
                {
                    NodeId = "node-789",
                    AreaIdentifier = "abc-123",
                    Fallback = false,
                    Text = "Is toby a lovely boy?",
                    IsRoot = false,
                    NodeChildrenString = "",
                    NodeType = "yesNo",
                    OptionPath = "No",
                    AccountId = accountId
                }
            };

            var areaOneMetas = new List<StaticTablesMeta>()
            {
                new StaticTablesMeta()
                {
                    AreaIdentifier = "abc-123",
                    AccountId = accountId,
                    Description = "This is a good first table.",
                    TableOrder = 0,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Damn Fee 1!",
                            Fee = new StaticFee() {FeeId = "0as", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = "abc-123",
                            AccountId = accountId
                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Damn Fee 2!",
                            Fee = new StaticFee() {FeeId = "1ewq", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = "abc-123",
                            AccountId = accountId

                        }
                    }
                },
                new StaticTablesMeta()
                {
                    AreaIdentifier = "abc-123",
                    AccountId = accountId,
                    Description = "This is a good second table.",
                    TableOrder = 1,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Damn Fee 1!",
                            Fee = new StaticFee() {FeeId = "0poi", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = "abc-123",
                            AccountId = accountId

                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Damn Fee 2!",
                            Fee = new StaticFee() {FeeId = "1iop", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = "abc-123",
                            AccountId = accountId

                        }
                    }
                },
            };
            var areaOne = new Area()
            {
                AreaIdentifier = "abc-123",
                AreaName = "First Area",
                Epilogue = "First Area Epilogue",
                Prologue = "First Area Prologue",
                EmailTemplate = "<h2>Wow!</h2>",
                ConversationNodes = areaOneNodes,
                StaticTablesMetas = areaOneMetas,
                GroupId = groupOneId,
                AccountId = accountId
            };
            
            
            var areaTwoNodes = new List<ConversationNode>()
            {
                new ConversationNode()
                {
                    NodeId = "node-987",
                    AreaIdentifier = "def-123",
                    Fallback = false,
                    Text = "Is toby a friendly boy?",
                    IsRoot = true,
                    NodeChildrenString = "",
                    NodeType = null,
                    OptionPath = null,
                    AccountId = accountId

                }
            };
            
            var areaTwoMetas = new List<StaticTablesMeta>()
            {
                new StaticTablesMeta()
                {
                    AreaIdentifier = "def-123",
                    AccountId = accountId,
                    Description = "This is an ok first table.",
                    TableOrder = 0,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Silly fee 1!",
                            Fee = new StaticFee() {FeeId = "0tyu", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = "def-123",
                            AccountId = accountId

                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Silly fee 2!",
                            Fee = new StaticFee() {FeeId = "1fgh", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = "def-123",
                            AccountId = accountId

                        }
                    }
                },
                new StaticTablesMeta()
                {
                    AreaIdentifier = "def-123",
                    AccountId = accountId,
                    Description = "This is a good second table.",
                    TableOrder = 1,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Angry Fee 1!",
                            Fee = new StaticFee() {FeeId = "0lmh", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = "def-123",
                            AccountId = accountId

                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Angry Fee 2!",
                            Fee = new StaticFee() {FeeId = "1vbh", Min = 0, Max = 12},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = "def-123",
                            AccountId = accountId

                        }
                    }
                },
            };
            
            var areaTwo = new Area()
            {
                AreaIdentifier = "def-123",
                AccountId = accountId,
                AreaName = "Second Area",
                Epilogue = "Second Area Epilogue",
                Prologue = "Second Area Prologue",
                EmailTemplate = "<h2>Wow! A great Email Template! Neat!</h2>",
                ConversationNodes = areaTwoNodes,
                StaticTablesMetas = areaTwoMetas,
                GroupId = areaTwoGroupId
            };

            Areas = new List<Area>()
            {
                areaOne, areaTwo
            };
        }
    }
}