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

        private void ThrowException(string emailAddress)
        {
            throw new DomainException($"The email address {emailAddress} is not allowed to be created in {determineCurrentEnvironment.Environment}");
        }

        public void CheckAccountAccess(string emailAddress)
        {
            if (determineCurrentEnvironment.IsStaging())
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
            return EmailIdentityList.AllowedEmailsInStaging.Contains(emailAddress);
        }
    }
}