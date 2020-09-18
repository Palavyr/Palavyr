﻿using System;
using System.Collections.Generic;
using Palavyr.API.Controllers;
using Server.Domain.conversations;

namespace Palavyr.API.controllers.accounts.seedData
{
    public class DevSeedData : BaseSeedData
    {
        public List<CompletedConversation> CompleteConversations { get; set; } = new List<CompletedConversation>();

        public DevSeedData(string accountId) : base(accountId)
        {
            CompleteConversations.Add(CompletedConversation.CreateNew("conv1-3234-b3jk-kb35", "resp1-2353-3532-345g.pdf", DateTime.Now, accountId, AreaName, EmailTemplate, false, "Toby", "toby@gmail.com", null));
            CompleteConversations.Add(CompletedConversation.CreateNew("conv2-3234-b3jk-kb35", "resp2-2353-3532-345g.pdf", DateTime.Now, accountId, AreaName, EmailTemplate, false, "Ana", "anagradie@gmail.com", "0449702364"));
        }
    }
}

