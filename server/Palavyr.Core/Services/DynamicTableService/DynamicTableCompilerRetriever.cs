using System;
using Autofac;
using Palavyr.Core.Services.DynamicTableService.Compilers;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableCompilerRetriever
    {
        private readonly ILifetimeScope lifetimeScope;

        public DynamicTableCompilerRetriever(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IDynamicTablesCompiler RetrieveCompiler(string dynamicTableName)
        {
            var compilerName = string.Join("", new[] {dynamicTableName, "Compiler"});
            var compilerType = Type.GetType(compilerName);
            if (compilerType == null)
            {
                throw new Exception($"Compiler type not found: {dynamicTableName}");
            }

            var compiler = (IDynamicTablesCompiler) lifetimeScope.Resolve(compilerType);
            return compiler;
        }
    }
}