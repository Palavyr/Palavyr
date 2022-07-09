using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static PricingStrategyTableBuilder<TResource> CreatePricingStrategyTableBuilder<TResource>(this BaseIntegrationFixture test) where TResource : class, IPricingStrategyTableRowResource, new()
        {
            return new PricingStrategyTableBuilder<TResource>(test);
        }
    }

    public class PricingStrategyTableBuilder<TResource> where TResource : class, IPricingStrategyTableRowResource, new()
    {
        private readonly BaseIntegrationFixture test;
        private readonly List<TResource> additionalRows = new List<TResource>();
        private string? intentId;

        public PricingStrategyTableBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public PricingStrategyTableBuilder<TResource> WithIntent(string intentId)
        {
            this.intentId = intentId;
            return this;
        }

        public PricingStrategyTableBuilder<TResource> WithRow(TResource rowResource)
        {
            additionalRows.Add(rowResource);
            return this;
        }

        public PricingStrategyTableBuilder<TResource> WithRow(Func<TResource> func)
        {
            var row = func.Invoke();
            additionalRows.Add(row);
            return this;
        }

        public PricingStrategyTableBuilder<TResource> WithRows(IEnumerable<TResource> rows)
        {
            additionalRows.AddRange(rows);
            return this;
        }

        public async Task<List<TResource>> Build()
        {
            return additionalRows;
        }

        /// <summary>
        /// Creating a table requires creating a meta as well as the table
        /// </summary>
        /// <returns></returns>
        public async Task<PricingStrategyTableMetaResource> BuildAndMake<TEntity, TCompiler>()
            where TEntity : class, IPricingStrategyTable<TEntity>, IEntity, new()
            where TCompiler : class, IPricingStrategyTableCompiler
        {
            var intentStore = test.ResolveStore<Area>();
            if (intentId is null)
            {
                var intent = await test.CreateIntentBuilder().Build();
                intentId = intent.AreaIdentifier;
            }
            else
            {
                var intent = await intentStore.GetOrNull(intentId, s => s.AreaIdentifier);
                if (intent is null)
                {
                    var newIntent = await test.CreateIntentBuilder().Build();
                    intentId = newIntent.AreaIdentifier;
                }
                else
                {
                    intentId = intent.AreaIdentifier;
                }
            }

            var fullRoute = UriUtils.Join(
                PricingStrategyControllerBase<TEntity, TResource, TCompiler>.BaseRoute,
                typeof(TEntity).Name,
                CreatePricingStrategyTableRequest<
                    TEntity,
                    TResource,
                    TCompiler>.FormatRoute(intentId)
            ).Replace("api/", "");

            var tableMeta = await test
                .Client
                .Post<CreatePricingStrategyTableRequest<
                        TEntity,
                        TResource,
                        TCompiler>,
                    PricingStrategyTableMetaResource>(
                    test.CancellationToken,
                    _ => fullRoute);

            if (additionalRows.Count > 0)
            {
                var getRoute = PricingStrategyControllerBase<TEntity, TResource, TCompiler>
                    .AssembleRoute<TEntity>(
                        GetPricingStrategyTableRowsRequest<
                            TEntity,
                            TResource,
                            TCompiler>.FormatRoute(intentId, tableMeta.TableId));

                var currentTable = await test.Client.GetResource<GetPricingStrategyTableRowsRequest<TEntity,
                    TResource,
                    TCompiler>, PricingStrategyTableDataResource<TResource>>(test.CancellationToken, _ => getRoute);

                foreach (var row in additionalRows)
                {
                    row.TableId = currentTable.TableRows.First().TableId;
                }

                currentTable.TableRows.ToList().AddRange(additionalRows);

                var saveRoute = PricingStrategyControllerBase<TEntity, TResource, TCompiler>
                    .AssembleRoute<TEntity>(
                        SavePricingStrategyTableRequest<TEntity, TResource, TCompiler>.FormatRoute(intentId, currentTable.TableRows.First().TableId)
                    );

                await test
                    .Client
                    .Post<SavePricingStrategyTableRequest<TEntity, TResource, TCompiler>, IEnumerable<TResource>>(
                        currentTable,
                        test.CancellationToken,
                        s => saveRoute);
            }

            return tableMeta;
        }
    }
}