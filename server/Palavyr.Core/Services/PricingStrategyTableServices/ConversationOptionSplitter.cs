﻿using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public interface IConversationOptionSplitter
    {
        string JoinValueOptions(string[] options);
        string JoinValueOptions(List<string> options);
        string JoinNodeChildrenArray(List<string> nodeChildrenParts);
        List<string> SplitValueOptions(string options);
        string GetTableIdFromPricingStrategyNodeType(string nodeName);
        string[] SplitNodeChildrenString(string nodeChildrenString);
    }

    public class ConversationOptionSplitter : IConversationOptionSplitter
    {
        private readonly IGuidFinder finder;

        public ConversationOptionSplitter(IGuidFinder finder)
        {
            this.finder = finder;
        }

        public string JoinValueOptions(string[] options)
        {
            var cleanList = options.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (cleanList.Length == 0)
            {
                return "";
            }
            else if (cleanList.Length == 1)
            {
                return cleanList.Single();
            }
            else
            {
                return string.Join(Delimiters.ValueOptionDelimiter, cleanList);
            }
        }

        public string JoinValueOptions(List<string> options)
        {
            return JoinValueOptions(options.ToArray());
        }

        public List<string> SplitValueOptions(string options)
        {
            return options.Split(Delimiters.ValueOptionDelimiter).ToList();
        }

        public string GetTableIdFromPricingStrategyNodeType(string nodeName) => finder.FindFirstGuidSuffixOrNull(nodeName);

        public string[] SplitNodeChildrenString(string nodeChildrenString)
        {
            return nodeChildrenString.Split(Delimiters.NodeChildrenStringDelimiter).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        public string JoinNodeChildrenArray(List<string> nodeChildrenParts)
        {
            var cleanList = nodeChildrenParts.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (cleanList.Length == 0)
            {
                return "";
            }
            else if (cleanList.Length == 1)
            {
                return cleanList.Single();
            }
            else
            {
                return string.Join(Delimiters.NodeChildrenStringDelimiter, cleanList);
            }
        }
    }
}