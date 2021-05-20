using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public interface IConversationOptionSplitter
    {
        string JoinValueOptions(string[] options);
        string JoinValueOptions(List<string> options);
        List<string> SplitValueOptions(string options);
        string GetTableIdFromDynamicNodeType(string nodeName);
        string[] SplitNodeChildrenString(string nodeChildrenString);
    }

    public class ConversationOptionSplitter : IConversationOptionSplitter
    {
        private readonly GuidFinder finder;

        public ConversationOptionSplitter(GuidFinder finder)
        {
            this.finder = finder;
        }

        public string JoinValueOptions(string[] options)
        {
            return string.Join(Delimiters.ValueOptionDelimiter, options);
        }

        public string JoinValueOptions(List<string> options)
        {
            return JoinValueOptions(options.ToArray());
        }

        public List<string> SplitValueOptions(string options)
        {
            return options.Split(Delimiters.ValueOptionDelimiter).ToList();
        }

        public string GetTableIdFromDynamicNodeType(string nodeName) => finder.FindGuid(nodeName);

        public string[] SplitNodeChildrenString(string nodeChildrenString) => nodeChildrenString.Split(Delimiters.NodeChildrenStringDelimiter);

        public string JoinNodeChildrenArray(List<string> nodeChildrenParts) => string.Join(Delimiters.NodeChildrenStringDelimiter, nodeChildrenParts);

    }
    
}