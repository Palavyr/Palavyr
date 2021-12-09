using System.Collections.Generic;
using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicResponseComponents
    {
        public IDynamicTablesCompiler Compiler { get; }
        public DynamicResponseParts Responses { get; }
        public string DynamicTableName { get; }
        public List<string> DynamicTableKeys { get; }

        public DynamicResponseComponents(IDynamicTablesCompiler compiler, DynamicResponseParts responses, string dynamicTableName, List<string> dynamicTableKeys)
        {
            Compiler = compiler;
            Responses = responses;
            DynamicTableName = dynamicTableName;
            DynamicTableKeys = dynamicTableKeys;
        }
    }
}