using System;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.Services.StripeServices
{
    public static class StripeExtensions
    {
        public static DateTime AddEndTimeBuffer(this Account.PaymentIntervalEnum paymentIntervalEnum, DateTime currentPeriodEnd)
        {
            switch (paymentIntervalEnum)
            {
                case (Account.PaymentIntervalEnum.Month):
                    return currentPeriodEnd.AddMonths(1);
                case (Account.PaymentIntervalEnum.Year):
                    return currentPeriodEnd.AddYears(1);
                default:
                    throw new Exception("Payment interval could not be determined");
            }
        }

        public static Account.PaymentIntervalEnum GetPaymentIntervalEnum(this string paymentInterval)
        {
            switch (paymentInterval)
            {
                case (Account.PaymentIntervals.Month):
                    return Account.PaymentIntervalEnum.Month;
                case (Account.PaymentIntervals.Year):
                    return Account.PaymentIntervalEnum.Year;
                default:
                    throw new Exception("Payment interval could not be determined");
            }
        }
    }
}