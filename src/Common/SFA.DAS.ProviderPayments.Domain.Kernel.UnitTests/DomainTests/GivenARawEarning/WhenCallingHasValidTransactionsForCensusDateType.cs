using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenARawEarning
{
    [TestFixture]
    public class WhenCallingHasValidTransactionsForCensusDateType
    {
        [TestFixture]
        public class WithTransactionType1
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType01 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType2
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType02 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, true)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType3
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType03 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, true)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType4
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType04 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, true)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType5
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType05 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, true)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType6
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType06 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, true)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType7
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType07 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, true)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType8
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType08 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType9
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType09 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, true)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType10
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType10 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }
                
                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, true)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType11
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType11 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType12
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType12 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType13
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType13 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType14
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType14 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithTransactionType15
        {
            private static readonly RawEarning TestEarning = new RawEarning { TransactionType15 = 100 };

            [TestFixture]
            public class AndNoDatesSet
            {
                [SetUp]
                public void Setup()
                {
                    TestEarning.EndDate = null;
                    TestEarning.FirstIncentiveCensusDate = null;
                    TestEarning.SecondIncentiveCensusDate = null;
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void Then(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);

                    actual.Should().Be(expected);
                }
            }
        }

        [TestFixture]
        public class WithAllDatesSet
        {
            private static readonly RawEarning TestEarning = new RawEarning
            {
                EndDate = new DateTime(),
                FirstIncentiveCensusDate = new DateTime(),
                SecondIncentiveCensusDate = new DateTime(),
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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void ThenNoIncentivesReturnTrue(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);
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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, true)]
                [TestCase(CensusDateType.Second16To18Incentive, true)]
                [TestCase(CensusDateType.CompletionPayments, true)]
                public void ThenNoIncentivesReturnTrue(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);
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
                }

                [TestCase(CensusDateType.OnProgLearning, false)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void ThenNoIncentivesReturnTrue(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);
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
                }

                [TestCase(CensusDateType.OnProgLearning, true)]
                [TestCase(CensusDateType.First16To18Incentive, false)]
                [TestCase(CensusDateType.Second16To18Incentive, false)]
                [TestCase(CensusDateType.CompletionPayments, false)]
                public void ThenNoIncentivesReturnTrue(CensusDateType censusDateType, bool expected)
                {
                    var actual = TestEarning.HasValidTransactionsForCensusDateType(censusDateType);
                    actual.Should().Be(expected);
                }
            }
        }
    }
}
