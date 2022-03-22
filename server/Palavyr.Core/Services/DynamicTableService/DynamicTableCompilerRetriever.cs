using System;
using System.Reflection;
using Autofac;

namespace Palavyr.Core.Services.DynamicTableService
{
    public interface IDynamicTableCompilerRetriever
    {
        IDynamicTablesCompiler RetrieveCompiler(string dynamicTableTypeName);
    }

    public class DynamicTableCompilerRetriever : IDynamicTableCompilerRetriever
    {
        private readonly ILifetimeScope lifetimeScope;

        public DynamicTableCompilerRetriever(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IDynamicTablesCompiler RetrieveCompiler(string dynamicTableTypeName)
        {
            var compilerType = Assembly.GetExecutingAssembly().GetType($"Palavyr.Core.Services.DynamicTableService.Compilers.I{dynamicTableTypeName}Compiler");
            // var ctype = compilerType.GetInterfaces().SingleOrDefault(x => x.Name.Contains(dynamicTableTypeName));
            if (compilerType is null)
            {
                throw new Exception($"Compiler type not found: {dynamicTableTypeName}");
            }
            else
            {
                var compiler = (IDynamicTablesCompiler)lifetimeScope.Resolve(compilerType);
                return compiler;
            }
        }

        public IDynamicTablesCompiler RetrieveCompiler<TCompiler>()
        {
            var compilerType = typeof(TCompiler);
            if (compilerType == null)
            {
                throw new Exception($"Compiler type not found: {compilerType.Name}");
            }

            var compiler = (IDynamicTablesCompiler)lifetimeScope.Resolve(compilerType);
            return compiler;
        }
    }
}