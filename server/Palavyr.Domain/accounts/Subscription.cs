﻿using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Accounts
{
    public class Subscription
    {
        
        [Key]
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string ApiKey { get; set; }
        public int NumAreas { get; set; }

        private Subscription(string accountId, string apiKey, int numAreas)
        {

            AccountId = accountId;
            ApiKey = apiKey;
            NumAreas = numAreas;
        }

        public static Subscription CreateNew(string accountId, string apiKey, int numAreas)
        {
            return new Subscription(accountId, apiKey, numAreas);
        }
    }
}