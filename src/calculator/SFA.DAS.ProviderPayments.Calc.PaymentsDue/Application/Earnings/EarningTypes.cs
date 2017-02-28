namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings
{
    public class EarningTypes
    {
        public const string Learning = "PriceEpisodeOnProgPayment";
        public const string Completion = "PriceEpisodeCompletionPayment";
        public const string Balancing = "PriceEpisodeBalancePayment";
        public const string First16To18EmployerIncentive = "PriceEpisodeFirstEmp1618Pay";
        public const string First16To18ProviderIncentive = "PriceEpisodeFirstProv1618Pay";
        public const string Second16To18EmployerIncentive = "PriceEpisodeSecondEmp1618Pay";
        public const string Second16To18ProviderIncentive = "PriceEpisodeSecondProv1618Pay";

        public const string Balancing16To18FrameworkUplift = "PriceEpisodeApplic1618FrameworkUpliftBalancing";
        public const string FrameworkUpliftCompletionPayment16To18 = "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment";
        public const string FrameworkUpliftOnProgPayment16To18 = "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment";
        public const string FirstDisadvantagePayment = "PriceEpisodeFirstDisadvantagePayment";
        public const string SecondDisadvantagePayment = "PriceEpisodeSecondDisadvantagePayment";

    }
}