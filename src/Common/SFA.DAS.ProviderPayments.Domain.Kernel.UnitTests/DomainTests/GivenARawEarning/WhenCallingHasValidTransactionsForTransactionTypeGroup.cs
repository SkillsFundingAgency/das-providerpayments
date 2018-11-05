using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenARawEarning
{
    [TestFixture]
    public partial class WhenCallingHasValidTransactionsForTransactionTypeGroup
    {

        [TestFixture]
        public class WithAllDatesSet
        {
            private static readonly RawEarning TestEarning = new RawEarning
            {
                EndDate = new DateTime(),
                FirstIncentiveCensusDate = new DateTime(),
                SecondIncentiveCensusDate = new DateTime(),
                LearnerAdditionalPaymentsDate = new DateTime(),
            };

            [TestFixture]
            public class WithNoAmounts
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.TransactionType01 = 0;
                    TestEarning.TransactionType02 = 0;
                    TestEarning.TransactionType03 = 0;
                    TestEarning.TransactionType04 = 0;
                    TestEarning.TransactionType05 = 0;
                    TestEarning.TransactionType06 = 0;
                    TestEarning.TransactionType07 = 0;
                    TestEarning.TransactionType08 = 0;
                    TestEarning.TransactionType09 = 0;
                    TestEarning.TransactionType10 = 0;
                    TestEarning.TransactionType11 = 0;
                    TestEarning.TransactionType12 = 0;
                    TestEarning.TransactionType13 = 0;
                    TestEarning.TransactionType14 = 0;
                    TestEarning.TransactionType15 = 0;
                    TestEarning.TransactionType16 = 0;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void ThenNoIncentivesReturnTrue(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);
                    actual.Should().Be(expected);
                }
            }

            [TestFixture]
            public class WithAllAmounts
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.TransactionType01 = 100;
                    TestEarning.TransactionType02 = 100;
                    TestEarning.TransactionType03 = 100;
                    TestEarning.TransactionType04 = 100;
                    TestEarning.TransactionType05 = 100;
                    TestEarning.TransactionType06 = 100;
                    TestEarning.TransactionType07 = 100;
                    TestEarning.TransactionType08 = 100;
                    TestEarning.TransactionType09 = 100;
                    TestEarning.TransactionType10 = 100;
                    TestEarning.TransactionType11 = 100;
                    TestEarning.TransactionType12 = 100;
                    TestEarning.TransactionType13 = 100;
                    TestEarning.TransactionType14 = 100;
                    TestEarning.TransactionType15 = 100;
                    TestEarning.TransactionType16 = 100;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, true)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
                [TestCase(TransactionTypeGroup.CompletionPayments, true)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, true)]
                public void ThenNoIncentivesReturnTrue(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);
                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithNoDatesSet
        {
            private static readonly RawEarning TestEarning = new RawEarning
            {
                EndDate = null,
                FirstIncentiveCensusDate = null,
                SecondIncentiveCensusDate = null,
                LearnerAdditionalPaymentsDate = null,
            };

            [TestFixture]
            public class WithNoAmounts
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.TransactionType01 = 0;
                    TestEarning.TransactionType02 = 0;
                    TestEarning.TransactionType03 = 0;
                    TestEarning.TransactionType04 = 0;
                    TestEarning.TransactionType05 = 0;
                    TestEarning.TransactionType06 = 0;
                    TestEarning.TransactionType07 = 0;
                    TestEarning.TransactionType08 = 0;
                    TestEarning.TransactionType09 = 0;
                    TestEarning.TransactionType10 = 0;
                    TestEarning.TransactionType11 = 0;
                    TestEarning.TransactionType12 = 0;
                    TestEarning.TransactionType13 = 0;
                    TestEarning.TransactionType14 = 0;
                    TestEarning.TransactionType15 = 0;
                    TestEarning.TransactionType16 = 0;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, false)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void ThenNoIncentivesReturnTrue(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);
                    actual.Should().Be(expected);
                }
            }

            [TestFixture]
            public class WithAllAmounts
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.TransactionType01 = 100;
                    TestEarning.TransactionType02 = 100;
                    TestEarning.TransactionType03 = 100;
                    TestEarning.TransactionType04 = 100;
                    TestEarning.TransactionType05 = 100;
                    TestEarning.TransactionType06 = 100;
                    TestEarning.TransactionType07 = 100;
                    TestEarning.TransactionType08 = 100;
                    TestEarning.TransactionType09 = 100;
                    TestEarning.TransactionType10 = 100;
                    TestEarning.TransactionType11 = 100;
                    TestEarning.TransactionType12 = 100;
                    TestEarning.TransactionType13 = 100;
                    TestEarning.TransactionType14 = 100;
                    TestEarning.TransactionType15 = 100;
                    TestEarning.TransactionType16 = 100;
                }

                [TestCase(TransactionTypeGroup.OnProgLearning, true)]
                [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
                [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
                [TestCase(TransactionTypeGroup.CompletionPayments, false)]
                [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
                public void ThenNoIncentivesReturnTrue(TransactionTypeGroup transactionTypeGroup, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup);
                    actual.Should().Be(expected);
                }
            }
        }
    }
}
