using System;
using Humanizer;

namespace Palavyr.Core.Services.EmailService
{
    public static class EmailConstants
    {
        public const string PalavyrMainEmailAddress = "palavyr@gmail.com";
        public const string PalavyrSubject = "Welcome to Palavyr - Email Verification";
        public const string PalavyrSubscriptionCreateSubject = "Thanks for subscribing to Palavyr!";
        public const string PalavyrPaymentFailedSubject = "Your recent payment to Palavyr.com failed. :(";
        public const string PalavyrPaymentSucceededSubject = "Thanks for you recent payment - From your friends at Palavyr.com";
        public static Func<DateTime?, string> PalavyrInvoiceCreatedSubject = dueDate => $"Your Palavyr Subscription Invoice for {dueDate.Humanize()}";
    }
}