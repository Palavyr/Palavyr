using System;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.API.Services.StripeServices
{
    public static class StripeExtensions
    {
        public static DateTime AddEndTimeBuffer(this UserAccount.PaymentIntervalEnum paymentIntervalEnum, DateTime currentPeriodEnd)
        {
            switch (paymentIntervalEnum)
            {
                case (UserAccount.PaymentIntervalEnum.Month):
                    return currentPeriodEnd.AddMonths(1);
                case (UserAccount.PaymentIntervalEnum.Year):
                    return currentPeriodEnd.AddYears(1);
                default:
                    throw new Exception("Payment interval could not be determined");
            }
        }

        public static UserAccount.PaymentIntervalEnum GetPaymentIntervalEnum(this string paymentInterval)
        {
            switch (paymentInterval)
            {
                case (UserAccount.PaymentIntervals.Month):
                    return UserAccount.PaymentIntervalEnum.Month;
                case (UserAccount.PaymentIntervals.Year):
                    return UserAccount.PaymentIntervalEnum.Year;
                default:
                    throw new Exception("Payment interval could not be determined");
            }
        }
    }
}