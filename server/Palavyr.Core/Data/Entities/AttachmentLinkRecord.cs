﻿
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class AttachmentLinkRecord : Entity, IHaveAccountId
    {
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}