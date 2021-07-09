using System;
using Autofac;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableCompilerRetriever
    {
        private readonly ILifetimeScope lifetimeScope;

        public DynamicTableCompilerRetriever(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IDynamicTablesCompiler RetrieveCompiler(string dynamicTableTypeName)
        {
            var compilerType = typeof(DynamicTableCompilerRetriever)
                .Assembly
                .GetType($"Palavyr.Core.Services.DynamicTableService.Compilers.{dynamicTableTypeName}Compiler");
            
            if (compilerType == null)
            {
                throw new Exception($"Compiler type not found: {dynamicTableTypeName}");
            }

            var compiler = (IDynamicTablesCompiler) lifetimeScope.Resolve(compilerType);
            return compiler;
        }
    }
}