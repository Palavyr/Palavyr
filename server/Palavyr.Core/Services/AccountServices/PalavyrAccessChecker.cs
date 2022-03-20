using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.AccountServices
{
    public class PalavyrAccessChecker : IPalavyrAccessChecker
    {
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;

        public PalavyrAccessChecker(IDetermineCurrentEnvironment determineCurrentEnvironment)
        {
            this.determineCurrentEnvironment = determineCurrentEnvironment;
        }


        public static void AssertEnvironmentsDoNoOverlap()
        {
            foreach (var email in EmailIdentityList.AllowedEmailsInDevelopment)
            {
                if (EmailIdentityList.AllowedEmailsInStaging.Contains(email))
                {
                    throw new PalavyrStartupException($"The allowed email lists in {nameof(PalavyrAccessChecker)} contain overlapped emails. This will cause stripe to blow up since they only offer dev and live.");
                }
            }
        }

        private void ThrowException(string emailAddress)
        {
            throw new DomainException($"The email address {emailAddress} is not allowed to be created in {determineCurrentEnvironment.Environment}");
        }

        public void CheckAccountAccess(string emailAddress)
        {
            if (determineCurrentEnvironment.IsDevelopment())
            {
                if (!EmailIdentityList.AllowedEmailsInDevelopment.Contains(emailAddress) && !emailAddress.StartsWith("test"))
                {
                    ThrowException(emailAddress);
                }
            }
            else if (determineCurrentEnvironment.IsStaging())
            {
                if (!EmailIdentityList.AllowedEmailsInStaging.Contains(emailAddress))
                {
                    ThrowException(emailAddress);
                }
            }
            else
            {
                // Do Nothing
            }
        }

        public bool IsATestStripeEmail(string emailAddress)
        {
            return (EmailIdentityList.AllowedEmailsInDevelopment.Contains(emailAddress) || EmailIdentityList.AllowedEmailsInStaging.Contains(emailAddress));
        }
    }
}