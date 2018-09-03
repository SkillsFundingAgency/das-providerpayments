using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes
{
    public class SetupPaymentsHistoryRepositoryAttribute : TestActionAttribute
    {
        private readonly long _idToSearchFor;

        public SetupPaymentsHistoryRepositoryAttribute(long idToSearchFor)
        {
            _idToSearchFor = idToSearchFor;
        }

        public override ActionTargets Targets { get; } = ActionTargets.Default;

        public override void BeforeTest(ITest test)
        {
            TestDataHelper.Execute("TRUNCATE TABLE Reference.PaymentsHistory", null, true);

            var fixture = new Fixture();

            var payments = new List<PaymentEntity>();
            for (var i = 0; i <= 2; i++)
            {
                var userPayments = fixture.Build<PaymentEntity>()
                    .With(x => x.Ukprn, _idToSearchFor)
                    .With(x => x.LearnRefNumber, "L" + i)
                    .With(x => x.FundingSource, FundingSource.CoInvestedEmployer)
                    .With(x => x.TransactionType, TransactionType.Learning)
                    .With(x => x.Amount, 100)
                    .CreateMany(3)
                    .ToList();

                var nonMatchingPayments = fixture.Build<PaymentEntity>()
                    .With(x => x.Ukprn, _idToSearchFor)
                    .With(x => x.LearnRefNumber, "L" + i)
                    .With(x => x.FundingSource, FundingSource.CoInvestedSfa)
                    .With(x => x.TransactionType, TransactionType.Learning)
                    .With(x => x.Amount, 100)
                    .CreateMany(3)
                    .ToList();
                payments.AddRange(userPayments);
                payments.AddRange(nonMatchingPayments);

            }

            foreach (var payment in payments)
            {
                AddHistoricPayment(payment);
            }

            base.BeforeTest(test);
        }

        static void AddHistoricPayment(PaymentEntity entity)
        {
            const string sql = @"
                    INSERT INTO Reference.PaymentsHistory
                        (PaymentId, RequiredPaymentId, DeliveryMonth, DeliveryYear,
                        CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, 
                        FundingSource, TransactionType, Amount, Ukprn, LearnRefNumber, FundingLineType)
                        VALUES
                        (NEWID(), @RequiredPaymentId, @DeliveryMonth, @DeliveryYear,
                        @CollectionPeriodName, @CollectionPeriodMonth, @CollectionPeriodYear, 
                        @FundingSource, @TransactionType, @Amount, @Ukprn, @LearnRefNumber, 'FundingLineType')
                ";

            TestDataHelper.Execute(sql, entity);
        }

    }
}