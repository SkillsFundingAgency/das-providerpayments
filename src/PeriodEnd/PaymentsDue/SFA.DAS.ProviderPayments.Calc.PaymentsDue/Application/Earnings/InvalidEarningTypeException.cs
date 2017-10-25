using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings
{
    public class InvalidEarningTypeException : Exception
    {
        public InvalidEarningTypeException(string earningType)
            : base($"The earning type must be PriceEpisodeOnProgPayment, PriceEpisodeCompletionPayment or PriceEpisodeBalancePayment but is {earningType}")
        {
        }
    }
}