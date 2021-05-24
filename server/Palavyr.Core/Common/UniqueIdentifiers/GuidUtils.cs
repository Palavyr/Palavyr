﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Palavyr.Core.Common.UniqueIdentifiers
{
    public static class GuidUtils
    {
        public static string CreateShortenedGuid([Range(1, 4)] int take)
        {
            var useNumParts = (take > 4) ? 4 : take;
            var ids = Guid.NewGuid().ToString().Split('-').Take(useNumParts).ToList();
            return string.Join('-', ids);
        }

        public static string CreatePseudoRandomString(int take)
        {
            var ids = string.Join("", Guid.NewGuid().ToString().Split('-'));
            var chars = string.Join("", ids.ToCharArray().Take(5));
            return chars;
        }

        public static string CreateNewId()
        {
            return Guid.NewGuid().ToString();
        }

    }
    
    public static class A 
    {
        public static string RandomName()
        {
            return Guid.NewGuid().ToString().Split('-').Take(1).ToList().Single();
        }
    }
}