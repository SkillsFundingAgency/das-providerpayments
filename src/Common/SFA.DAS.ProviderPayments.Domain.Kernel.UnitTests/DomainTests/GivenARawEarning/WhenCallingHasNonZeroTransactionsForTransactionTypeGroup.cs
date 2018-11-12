using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenARawEarning
{
    [TestFixture]
    public class WhenCallingHasNonZeroTransactionsForTransactionTypeGroup
    {
        [TestFixture]
        public class WithTransactionType1
        {
            private static readonly RawEarning TestEarning = new RawEarning {TransactionType01 = 100};

            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType2
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType02 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType3
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType03 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType4
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType04 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType5
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType05 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType6
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType06 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType7
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType07 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType8
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType08 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType9
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType09 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType10
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType10 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType11
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType11 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType12
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType12 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType13
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType13 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType14
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType14 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType15
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType15 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, false)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType16
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType16 = 100 };

            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            [TestCase(TransactionTypeGroup.SixtyDayIncentives, true)]
            public void ThenTestByCensusDateType(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TestEarning.HasNonZeroTransactionsForGroup(transactionTypeGroup);

                actual.Should().Be(expected);
            }
        }
    }
}
