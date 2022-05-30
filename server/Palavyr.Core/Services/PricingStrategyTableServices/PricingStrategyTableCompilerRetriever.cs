using System;
using System.Reflection;
using Autofac;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IPricingStrategyTableCompilerRetriever
    {
        IPricingStrategyTableCompiler RetrieveCompiler(string pricingStrategyTableType);
        IPricingStrategyTableCompiler RetrieveCompiler<TCompiler>() where TCompiler : class, IPricingStrategyTableCompiler;
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

        public IPricingStrategyTableCompiler RetrieveCompiler<TCompiler>() where TCompiler : class, IPricingStrategyTableCompiler
        {
            return lifetimeScope.Resolve<TCompiler>();
        }
    }
}