using System;
using System.Collections.Generic;
using Server.Domain;
using Server.Domain.App.schema;
using Server.Domain.DynamicTables;

namespace DashboardServer.API.Controllers
{
    public class DevData
    {
        public List<Area> Areas { get; set; }
        public List<GroupMap> Groups { get; set; } = new List<GroupMap>();
        public WidgetPreference WidgetPreference { get; set; }
        public readonly List<SelectOneFlat> SelectOneFlatsDefaultData = new List<SelectOneFlat>();
        public List<CompletedConversation> CompleteConversations { get; set; } = new List<CompletedConversation>();
        
        public const string Area1 = "abc-123";
        public const string Area2 = "def-123";
        public const string AreaName1 = "First Area";
        public const string AreaName2 = "Second Area";

        public const string Area1DynamicTableId = "h1h3-5k32k-5h3k-35l3";
        public const string Area2DynamicTableId = "1swe-3gg5-k7hh-2sde";
        public const string TableName1 = "puppies";
        public const string TableName2 = "retainers";

        public const string Email1 = "<div> Howdy! </div>";
        public const string Email2 = "<div> WOOOT! </div>";

        public string AccountId { get; set; }
        public List<DynamicTableMeta> DynamicTableMetas { get; set; } = new List<DynamicTableMeta>();

        
        private void SetDynamicDefaults()
        {
            SelectOneFlatsDefaultData.Add(SelectOneFlat.CreateNew("dashboardDev", Area1, "Red Cavvy", 924.00, 1200.00, true, Area1DynamicTableId, TableName1));
            SelectOneFlatsDefaultData.Add(SelectOneFlat.CreateNew("dashboardDev", Area1, "Blue Cavvy", 24.00, 50.00, false, Area1DynamicTableId, TableName1));

            SelectOneFlatsDefaultData.Add(SelectOneFlat.CreateNew("dashboardDev", Area2, "Plastic Retainers", 24.50, 0.00, false, Area2DynamicTableId, TableName2));
            SelectOneFlatsDefaultData.Add(SelectOneFlat.CreateNew("dashboardDev", Area2, "Wire Retainers", 53.75, 0.00, false, Area2DynamicTableId, TableName2));

        }
        
        private void SetCompletedConvos()
        {
            CompleteConversations.Add(CompletedConversation.CreateNew("conv1-3234-b3jk-kb35", "resp1-2353-3532-345g.pdf", DateTime.Now, AccountId, AreaName1, Email1, false, "Toby", "toby@gmail.com", null));
            CompleteConversations.Add(CompletedConversation.CreateNew("conv2-3234-b3jk-kb35", "resp2-2353-3532-345g.pdf", DateTime.Now, AccountId, AreaName1, Email1, false, "Ana", "anagradie@gmail.com", "0449702364"));
            CompleteConversations.Add(CompletedConversation.CreateNew("conv3-3234-b3jk-kb35", "resp3-2353-3532-345g.pdf", DateTime.Now, AccountId, AreaName2, Email2, true, "Paul", "paul.e.gradie@gmail.com", "0988324567"));
            CompleteConversations.Add(CompletedConversation.CreateNew("conv4-3234-b3jk-kb35", "resp4-2353-3532-345g.pdf", DateTime.Now, AccountId, AreaName2, Email2, false, "BabaJooon", "babajoon@gmail.com", "0430283242"));
        }

        private void SetDynamicTableMetas()
        {
            DynamicTableMetas = new List<DynamicTableMeta>()
            {
                DynamicTableMeta.CreateNew(TableName1, DynamicTableTypes.DefaultPrettyName, DynamicTableTypes.DefaultTable, Area1DynamicTableId, Area1,
                    AccountId),
                DynamicTableMeta.CreateNew(TableName2, DynamicTableTypes.DefaultPrettyName, DynamicTableTypes.DefaultTable, Area2DynamicTableId, Area2,
                    AccountId)
            };
        }

        public DevData(string accountId = "dashboardDev")
        {

            AccountId = accountId;
            
            SetCompletedConvos();
            SetDynamicDefaults();
            SetDynamicTableMetas();
            
            var groupOneId = Guid.NewGuid().ToString();
            string areaTwoGroupId = null;
            
            WidgetPreference = WidgetPreference.CreateNew(
                "Pauls Dev Company", 
                "Powerful Engagement", 
                "Write here...", 
                false,
                AccountId
                );
            
            Groups.Add(GroupMap.CreateGroupMap(groupOneId, null, "Group One", AccountId)); ;
            var areaOneNodes = new List<ConversationNode>()
            {
                new ConversationNode()
                {
                    NodeId = "node-123",
                    AreaIdentifier = Area1,
                    Fallback = false,
                    Text = "Is toby a good boy?",
                    IsRoot = true,
                    NodeChildrenString = "", //"node-456,node-789",
                    NodeType = null,
                    OptionPath = null,
                    ValueOptions = null,
                    AccountId = AccountId
                }
            };

            var areaOneMetas = new List<StaticTablesMeta>()
            {
                new StaticTablesMeta()
                {
                    AreaIdentifier = Area1,
                    AccountId = AccountId,
                    Description = "This is a good first table.",
                    TableOrder = 0,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Damn Fee 1!",
                            Fee = new StaticFee() {FeeId = "0as", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = Area1,
                            AccountId = AccountId

                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Damn Fee 2!",
                            Fee = new StaticFee() {FeeId = "1ewq", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = Area1,
                            AccountId = AccountId
                        }
                    }
                },
                new StaticTablesMeta()
                {
                    AreaIdentifier = Area1,
                    AccountId = AccountId,
                    Description = "This is a good second table.",
                    TableOrder = 1,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Damn Fee 1!",
                            Fee = new StaticFee() {FeeId = "0poi", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = Area1,
                            AccountId = AccountId
                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Damn Fee 2!",
                            Fee = new StaticFee() {FeeId = "1iop", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = Area1,
                            AccountId = AccountId

                        }
                    }
                },
            };
            var areaOne = new Area()
            {
                AreaIdentifier = Area1,
                AccountId = AccountId,
                AreaName = AreaName1,
                AreaDisplayTitle = "HouseKeeping",
                Epilogue = "First Area Epilogue",
                Prologue = "First Area Prologue",
                EmailTemplate = "<h2>Wow!</h2>",
                ConversationNodes = areaOneNodes,
                StaticTablesMetas = areaOneMetas,
                DynamicTableMetas = new List<DynamicTableMeta>() {DynamicTableMetas[0]},
                GroupId = groupOneId
            };
            
            
            var areaTwoNodes = new List<ConversationNode>()
            {
                new ConversationNode()
                {
                    NodeId = "node-987",
                    AreaIdentifier = Area2,
                    AccountId = AccountId,
                    Fallback = false,
                    Text = "Is toby a friendly boy?",
                    IsRoot = true,
                    NodeChildrenString = "",
                    NodeType = null,
                    OptionPath = null,
                    ValueOptions = null
                }
            };
            
            var areaTwoMetas = new List<StaticTablesMeta>()
            {
                new StaticTablesMeta()
                {
                    AreaIdentifier = Area2,
                    AccountId = AccountId,
                    Description = "This is an ok first table.",
                    TableOrder = 0,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Silly fee 1!",
                            Fee = new StaticFee() {FeeId = "0tyu", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = Area2,
                            AccountId = AccountId

                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Silly fee 2!",
                            Fee = new StaticFee() {FeeId = "1fgh", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 0,
                            AreaIdentifier = Area2,
                            AccountId = AccountId

                        }
                    }
                },
                new StaticTablesMeta()
                {
                    AreaIdentifier = Area2,
                    AccountId = AccountId,
                    Description = "This is a good second table.",
                    TableOrder = 1,
                    StaticTableRows = new List<StaticTableRow>()
                    {
                        new StaticTableRow()
                        {
                            RowOrder = 0,
                            Description = "Angry Fee 1!",
                            Fee = new StaticFee() {FeeId = "0lmh", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = Area2,
                            AccountId = AccountId

                        },
                        new StaticTableRow()
                        {
                            RowOrder = 1,
                            Description = "Angry Fee 2!",
                            Fee = new StaticFee() {FeeId = "1vbh", Min = 0.00, Max = 12.00},
                            Range = true,
                            PerPerson = true,
                            TableOrder = 1,
                            AreaIdentifier = Area2,
                            AccountId = AccountId

                        }
                    }
                },
            };
            
            var areaTwo = new Area()
            {
                AreaIdentifier = Area2,
                AccountId = AccountId,
                AreaName = AreaName2,
                AreaDisplayTitle = "Breadmaking",
                Epilogue = "Second Area Epilogue",
                Prologue = "Second Area Prologue",
                EmailTemplate = "<h2>Wow! A great Email Template! Neat!</h2>",
                ConversationNodes = areaTwoNodes,
                StaticTablesMetas = areaTwoMetas,
                DynamicTableMetas = new List<DynamicTableMeta>() {DynamicTableMetas[1]},
                GroupId = areaTwoGroupId
            };

            Areas = new List<Area>()
            {
                areaOne, areaTwo
            };
        }
    }
}