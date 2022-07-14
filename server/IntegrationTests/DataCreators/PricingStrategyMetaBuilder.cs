// using System.Threading.Tasks;
// using Palavyr.Core.Handlers.ControllerHandler;
// using Palavyr.Core.Models.Configuration.Schemas;
// using Palavyr.Core.Models.Contracts;
// using Palavyr.Core.Resources.PricingStrategyResources;
// using Palavyr.Core.Services.PricingStrategyTableServices;
// using IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
// using Test.Common.Random;
//
// namespace IntegrationTests.DataCreators
// {
//     public static partial class BuilderExtensionMethods
//     {
//         public static PricingStrategyMetaBuilder CreatePricingStrategyMetaBuilder(this BaseIntegrationFixture test)
//         {
//             return new PricingStrategyMetaBuilder(test);
//         }
//     }
//
//     public class PricingStrategyMetaBuilder
//     {
//         private readonly BaseIntegrationFixture test;
//         private string? tableTag;
//         private string? prettyName;
//         private string? tableType;
//         private string? tableId;
//         private string? accountId;
//         private string? intentId;
//         bool? valuesAsPaths = false; // for tables that specify various options, whether or not to use each option to create a new tree path.
//         bool? useTableTagAsResponseDescription = false;
//         private UnitIds? unitId;
//
//         public PricingStrategyMetaBuilder(BaseIntegrationFixture test)
//         {
//             this.test = test;
//         }
//
//         public PricingStrategyMetaBuilder WithAccountId(string accountId)
//         {
//             this.accountId = accountId;
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithIntentId(string intentId)
//         {
//             this.intentId = intentId;
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithTableTag(string tag)
//         {
//             this.tableTag = tag;
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithPrettyName(IHaveAPrettyNameAndTableType name)
//         {
//             this.prettyName = name.GetPrettyName();
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithTableType(IHaveAPrettyNameAndTableType tableType)
//         {
//             // this shold be restricted to available types?
//             this.tableType = tableType.GetTableType();
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithTableId(string tableId)
//         {
//             this.tableId = tableId;
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithValueAsPaths()
//         {
//             this.valuesAsPaths = true;
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithTableTagAsResponseDescription()
//         {
//             this.useTableTagAsResponseDescription = true;
//             return this;
//         }
//
//         public PricingStrategyMetaBuilder WithUnitId(UnitIds unitid)
//         {
//             this.unitId = unitid;
//             return this;
//         }
//
//         public async Task<DynamicTableMeta> Build()
//         {
//             var accId = this.accountId ?? test.AccountId;
//
//             // private readonly BaseIntegrationFixture test;
//             // private string? tableTag;
//             // private string? prettyName;
//             // private string? tableType;
//             // private string? tableId;
//             // private string? accountId;
//             // private string? intentId;
//             // bool? valuesAsPaths = false; // for tables that specify various options, whether or not to use each option to create a new tree path.
//             // bool? useTableTagAsResponseDescription = false;
//             // private UnitIds? unitId;
//
//             var intentId = this.intentId ?? (await test.CreateIntentBuilder().WithAccountId(accId).Build()).AreaIdentifier;
//             var tag = this.tableTag ?? A.RandomName();
//             var pname = this.prettyName ?? A.RandomName();
//             // var tType = this.tableType ?? "SelectOneFlat"; // TODO - nah
//             var tId = this.tableId ?? A.RandomId();
//             var vap = this.valuesAsPaths ?? false;
//             var uAR = this.useTableTagAsResponseDescription ?? false;
//             var unitId = this.unitId ?? UnitIds.Currency;
//
//             await Task.Yield();
//             return new DynamicTableMeta
//             {
//                 AccountId = accountId ?? test.AccountId,
//                 AreaIdentifier = intentId,
//                 TableTag = tag,
//                 PrettyName = pname,
//                 TableType = tType,
//                 TableId = tId,
//                 ValuesAsPaths = vap,
//                 UseTableTagAsResponseDescription = uAR,
//                 UnitId = unitId
//             };
//         }
//
//         public async Task<DynamicTableMeta> BuildAndMake<TableType, TResponse, TCompiler>()
//             where TableType : class, IPricingStrategyTable<TableType>, IEntity, new()
//             where TResponse : IPricingStrategyTableRowResource
//             where TCompiler : IPricingStrategyTableCompiler
//         {
//             var resource = await Build();
//             var result = await test.Client.Post<CreatePricingStrategyTableRequest<TableType, TResponse, TCompiler>, CreatePricingStrategyTableResponse<TResponse>>(resource, test.CancellationToken);
//             return result.Response;
//         }
//     }
// }