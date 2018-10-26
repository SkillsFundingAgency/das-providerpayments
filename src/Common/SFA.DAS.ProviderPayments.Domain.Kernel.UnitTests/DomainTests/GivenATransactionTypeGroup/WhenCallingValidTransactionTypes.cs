using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.UnitTests.DomainTests.GivenATransactionTypeGroup
{
    [TestFixture]
    class WhenCallingValidTransactionTypes
    {
        [Test]
        public void ThenShouldNotReturnTheSameResultForMoreThanOneValue()
        {
            var allTransactions = new HashSet<TransactionType>();

            foreach (TransactionTypeGroup transactionTypeGroup in Enum.GetValues(typeof(TransactionTypeGroup)))
            {
                var newValues = transactionTypeGroup.ValidTransactionTypes();
                foreach (var transactionType in newValues)
                {
                    allTransactions.Should().NotContain(transactionType);
                    allTransactions.Add(transactionType);
                }
            }
        }

        [Test]
        public void ThenAllTransactionTypeGroupsShouldReturnAllTransactionTypes()
        {
            var expected = new HashSet<TransactionType>();

            foreach (TransactionTypeGroup transactionTypeGroup in Enum.GetValues(typeof(TransactionTypeGroup)))
            {
                var newValues = transactionTypeGroup.ValidTransactionTypes();
                foreach (var transactionType in newValues)
                {
                    expected.Add(transactionType);
                }
            }

            var actual = Enum.GetValues(typeof(TransactionType));
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
