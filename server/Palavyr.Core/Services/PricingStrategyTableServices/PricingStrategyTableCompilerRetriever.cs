using System;
using System.Reflection;
using Autofac;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyTableCompilerRetriever
    {
        IPricingStrategyTableCompiler RetrieveCompiler(string pricingStrategyTableType);
        IPricingStrategyTableCompiler RetrieveCompiler<TPricingStrategy>() where TPricingStrategy : class, IPricingStrategyTable<TPricingStrategy>;
    }

    public class PricingStrategyTableCompilerRetriever : IPricingStrategyTableCompilerRetriever
    {
        private readonly ILifetimeScope lifetimeScope;

        public PricingStrategyTableCompilerRetriever(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IPricingStrategyTableCompiler RetrieveCompiler(string pricingStrategyTableType)
        {
            var compilerType = Assembly.GetExecutingAssembly().GetType($"Palavyr.Core.Services.DynamicTableService.Compilers.I{pricingStrategyTableType}Compiler");
            // var ctype = compilerType.GetInterfaces().SingleOrDefault(x => x.Name.Contains(dynamicTableTypeName));
            if (compilerType is null)
            {
                throw new Exception($"Compiler type not found: {pricingStrategyTableType}");
            }
            else
            {
                var compiler = (IPricingStrategyTableCompiler)lifetimeScope.Resolve(compilerType);
                return compiler;
            }
        }

        public IPricingStrategyTableCompiler RetrieveCompiler<TPricingStrategy>() where TPricingStrategy : class, IPricingStrategyTable<TPricingStrategy>
        {
            return (IPricingStrategyTableCompiler)lifetimeScope.Resolve(typeof(TPricingStrategy));
        }
    }
}