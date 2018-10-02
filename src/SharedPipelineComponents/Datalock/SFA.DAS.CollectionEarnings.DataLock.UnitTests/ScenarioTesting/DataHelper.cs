using System;
using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using TechTalk.SpecFlow;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.ScenarioTesting
{
    public static class DataHelper
    {

        public static List<CommitmentEntity> CreateCommitmentEntities(Table table)
        {
            var retVal = new List<CommitmentEntity>();

            foreach (var row in table.Rows)
            {
                var commitment = new CommitmentEntity
                {
                    CommitmentId = row.CastFieldAs<long>("CommitmentId"),
                    VersionId = row.CastFieldAs<string>("VersionId"),
                    AccountId = row.CastFieldAs<long>("AccountId"),
                    StartDate = row.CastFieldAs<DateTime>("StartDate"),
                    EndDate = row.CastFieldAs<DateTime>("EndDate"),
                    AgreedCost = row.CastFieldAs<decimal>("AgreedCost"),
                    StandardCode = row.CastFieldAs<long?>("Standard"),
                    ProgrammeType = row.CastFieldAs<int?>("Prog"),
                    FrameworkCode = row.CastFieldAs<int?>("Framework"),
                    PathwayCode = row.CastFieldAs<int?>("Pathway"),
                    PaymentStatus = row.CastFieldAs<int>("PaymentStatus"),
                    PaymentStatusDescription = row.CastFieldAs<string>("PaymentStatusDescription"),
                    EffectiveFrom = row.CastFieldAs<DateTime>("EffectiveFromDate"),
                    EffectiveTo = row.CastFieldAs<DateTime>("EffectiveToDate"),
                    WithdrawnOnDate = row.CastFieldAs<DateTime?>("WithdrawnOnDate"),
                    TransferSendingEmployerAccountId = row.CastFieldAs<long?>("TransferSendingEmployerAccountId"),
                    TransferApprovalDate = row.CastFieldAs<DateTime?>("TransferApprovalDate"),
                    Uln = row.CastFieldAs<long>("Uln", 100),
                };
                retVal.Add(commitment);
            }

            return retVal;
        }

        public static List<RawEarning> CreateRawEarnings(Table table)
        {
            var retVal = new List<RawEarning>();

            foreach (var row in table.Rows)
            {
                var earning = new RawEarning
                {
                    PriceEpisodeIdentifier = row.CastFieldAs<string>("PriceEpisodeIdentifier"),
                    EpisodeStartDate = row.CastFieldAs<DateTime?>("EpisodeStartDate"),
                    EpisodeEffectiveTnpStartDate = row.CastFieldAs<DateTime?>("TNPStartDate"),
                    AgreedPrice = row.CastFieldAs<decimal>("AgreedPrice"),
                    Period = row.CastFieldAs<int>("Period"),
                    ProgrammeType = row.CastFieldAs<int>("Prog"),
                    PathwayCode = row.CastFieldAs<int>("Pathway"),
                    StandardCode = row.CastFieldAs<int>("Standard"),
                    FrameworkCode = row.CastFieldAs<int>("Framework"),
                    SfaContributionPercentage = row.CastFieldAs<decimal>("SfaContributionPercentage"),
                    FundingLineType = row.CastFieldAs<string>("FundingLineType"),
                    LearningStartDate = row.CastFieldAs<DateTime>("LearningStartDate"),
                    LearnAimRef = row.CastFieldAs<string>("LearnAimRef"),
                    TransactionType01 = row.CastFieldAs<decimal>("TT01"),
                    TransactionType02 = row.CastFieldAs<decimal>("TT02"),
                    TransactionType03 = row.CastFieldAs<decimal>("TT03"),
                    TransactionType04 = row.CastFieldAs<decimal>("TT04"),
                    TransactionType05 = row.CastFieldAs<decimal>("TT05"),
                    TransactionType06 = row.CastFieldAs<decimal>("TT06"),
                    TransactionType07 = row.CastFieldAs<decimal>("TT07"),
                    TransactionType08 = row.CastFieldAs<decimal>("TT08"),
                    TransactionType09 = row.CastFieldAs<decimal>("TT09"),
                    TransactionType10 = row.CastFieldAs<decimal>("TT10"),
                    TransactionType11 = row.CastFieldAs<decimal>("TT11"),
                    TransactionType12 = row.CastFieldAs<decimal>("TT12"),
                    TransactionType15 = row.CastFieldAs<decimal>("TT15"),
                    ApprenticeshipContractType = row.CastFieldAs<int>("ApprenticeshipContractType", 1),
                    Uln = row.CastFieldAs<long>("Uln", 100),
                    EndDate = row.CastFieldAs<DateTime?>("End Date"),
                    SecondIncentiveCensusDate = row.CastFieldAs<DateTime?>("2nd Incentive Date"),
                    FirstIncentiveCensusDate = row.CastFieldAs<DateTime?>("1st Incentive Date"),
                };
                if ((earning.TransactionType04 != 0 || earning.TransactionType05 != 0) &&
                    earning.FirstIncentiveCensusDate == null)
                {
                    throw new Exception("Please include a column: '1st Incentive Date' to use transaction types 4 or 5");
                }
                if ((earning.TransactionType06 != 0 || earning.TransactionType07 != 0) &&
                    earning.SecondIncentiveCensusDate == null)
                {
                    throw new Exception("Please include a column: '2nd Incentive Date' to use transaction types 6 or 7");
                }
                retVal.Add(earning);
            }

            return retVal;
        }
    }
}