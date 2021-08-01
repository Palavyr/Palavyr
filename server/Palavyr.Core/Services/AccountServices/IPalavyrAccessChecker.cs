namespace Palavyr.Core.Services.AccountServices
{
    public interface IPalavyrAccessChecker
    {
        static void AssertEnvironmentsDoNoOverlap()
        {
            throw new System.NotImplementedException();
        }

        void CheckAccountAccess(string emailAddress);
        bool IsATestStripeEmail(string emailAddress);

    }
}