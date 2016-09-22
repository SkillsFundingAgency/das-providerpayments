using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    //public class WhenDpp195AcceptanceScenario2
    //{
    //    private CoInvestedPaymentsTask _uut;
    //    private IntegrationTaskContext _taskContext;
    //    private string _commitmentId;

    //    [SetUp]
    //    public void Arrange()
    //    {
    //        TestDataHelper.Clean();

    //        var accountId = Guid.NewGuid().ToString();
    //        TestDataHelper.AddAccount(accountId);

    //        _commitmentId = Guid.NewGuid().ToString();
    //        TestDataHelper.AddCommitment(_commitmentId, accountId);

    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 9, deliveryYear: 17, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 10, deliveryYear: 17, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 11, deliveryYear: 17, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 12, deliveryYear: 17, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 1, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 2, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 3, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 4, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 5, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 6, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 7, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 8, deliveryYear: 18, amountDue: 923.07692, transactionType: TransactionType.Learning);
    //        TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 9, deliveryYear: 18, amountDue: 3923.07692, transactionType: TransactionType.Learning);

    //        _taskContext = new IntegrationTaskContext();
    //        _uut = new CoInvestedPaymentsTask();
    //    }

    //    [Test]
    //    public void The26PaymentsAreMade()
    //    {
    //        Act();

    //        var paymentsCount = TestDataHelper.GetPaymentsCount();
    //        Assert.IsNotNull(paymentsCount);
    //        Assert.AreEqual(26, paymentsCount);
    //    }
    //    [Test]
    //    public void ThenResultShouldBe_()
    //    {
    //        Act();

    //        var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
    //        Assert.IsNotNull(payments);
    //        Assert.AreEqual(12, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 830.76923));
    //        Assert.AreEqual(12, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 92.30769));
    //        Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 3530.76923));
    //        Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 392.30769));
    //    }

    //    private void Act()
    //    {
    //        _uut.Execute(_taskContext);
    //    }
    //}

}
