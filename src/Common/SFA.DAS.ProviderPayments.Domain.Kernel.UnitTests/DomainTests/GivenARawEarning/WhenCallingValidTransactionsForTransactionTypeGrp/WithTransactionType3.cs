using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

// ReSharper disable once CheckNamespace
namespace SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenARawEarning
{
    [TestFixture]
    public partial class WhenCallingHasValidTransactionsForTransactionTypeGroup
    {
        [TestFixture]
        public class WithTransactionType3
        {
            private static readonly RawEarning TestEarning = new RawEarning {TransactionType03 = 100};

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                    TestEarning.LearnerAdditionalPaymentsDate = null;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void Then(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);

                    actual.Should().Be(expected);
                }
            }

            [TestFixture]
            public class AndFirst16To18IncentiveDateSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                    TestEarning.FirstIncentiveCensusDate = new DateTime();
                    TestEarning.LearnerAdditionalPaymentsDate = null;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void Then(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);

                    actual.Should().Be(expected);
                }
            }

            public class AndSecond16To18IncentiveDateSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = new DateTime();
                    TestEarning.LearnerAdditionalPaymentsDate = null;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void Then(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);

                    actual.Should().Be(expected);
                }
            }

            public class AndEndDateSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                    TestEarning.EndDate = new DateTime();
                    TestEarning.LearnerAdditionalPaymentsDate = null;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, true)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void Then(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);

                    actual.Should().Be(expected);
                }
            }

            public class AndSixtyDayDateSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                    TestEarning.EndDate = null;
                    TestEarning.LearnerAdditionalPaymentsDate = new DateTime();
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void Then(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);

                    actual.Should().Be(expected);
                }
            }
        }
    }
}