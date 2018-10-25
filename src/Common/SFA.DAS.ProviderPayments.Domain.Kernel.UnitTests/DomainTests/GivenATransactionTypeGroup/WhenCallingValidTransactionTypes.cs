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

            foreach (TransactionTypeGroup censusDateType in Enum.GetValues(typeof(TransactionTypeGroup)))
            {
                var newValues = censusDateType.ValidTransactionTypes();
                foreach (var transactionType in newValues)
                {
                    allTransactions.Should().NotContain(transactionType);
                    allTransactions.Add(transactionType);
                }
            }
        }
    }
}
