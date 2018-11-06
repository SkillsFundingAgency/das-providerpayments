namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.FutureDomain
{
    class CollectionPeriod
    {
        public CollectionPeriod(string name)
        {
            Name = name;
            AcademicYear = Name.Substring(0, 4);
        }

        public string Name { get; }
        public string AcademicYear { get; }
    }
}
