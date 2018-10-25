using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;
using SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenARawEarning;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenATransactionType
{
    [TestFixture]
    public class WhenCallingValidForTransactionTypeGroup
    {
        [TestFixture]
        public class WithTransactionType1
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Learning.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType2
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Completion.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType3
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Balancing.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType4
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.First16To18EmployerIncentive.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType5
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.First16To18ProviderIncentive.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType6
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Second16To18EmployerIncentive.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType7
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Second16To18ProviderIncentive.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType8
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.OnProgramme16To18FrameworkUplift.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType9
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Completion16To18FrameworkUplift.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType10
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, true)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.Balancing16To18FrameworkUplift.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType11
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, true)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.FirstDisadvantagePayment.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType12
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, false)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, true)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.SecondDisadvantagePayment.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType13
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.OnProgrammeMathsAndEnglish.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType14
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.BalancingMathsAndEnglish.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }

        [TestFixture]
        public class WithTransactionType15
        {
            [TestCase(TransactionTypeGroup.OnProgLearning, true)]
            [TestCase(TransactionTypeGroup.NinetyDayIncentives, false)]
            [TestCase(TransactionTypeGroup.ThreeSixtyFiveDayIncentives, false)]
            [TestCase(TransactionTypeGroup.CompletionPayments, false)]
            public void ThenTheResultIsCorrect(TransactionTypeGroup transactionTypeGroup, bool expected)
            {
                var actual = TransactionType.LearningSupport.ValidForTransactionTypeGroup(transactionTypeGroup);
                actual.Should().Be(expected);
            }
        }
    }
}
