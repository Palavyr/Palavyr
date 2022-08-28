using System.Collections.Generic;

namespace Palavyr.Core.Services.AccountServices
{
    public static class EmailIdentityList
    {
        public static List<string> AllowedEmailsInStaging { get; set; } = new List<string>()
        {
            "ana.gradie@gmail.com",
            "anasadeghi15@gmail.com",
            "palavyr.demo@gmail.com", // allowed in staging and prod,
            "pkstarstorm05@gmail.com"
        };
    }
}