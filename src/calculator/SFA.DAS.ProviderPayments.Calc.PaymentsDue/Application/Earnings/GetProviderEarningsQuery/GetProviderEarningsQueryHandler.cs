using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryHandler : IRequestHandler<GetProviderEarningsQueryRequest, GetProviderEarningsQueryResponse>
    {
        private readonly IEarningRepository _earningRepository;

        public GetProviderEarningsQueryHandler(IEarningRepository earningRepository)
        {
            _earningRepository = earningRepository;
        }

        public GetProviderEarningsQueryResponse Handle(GetProviderEarningsQueryRequest message)
        {
            try
            {
                var earningEntities = _earningRepository.GetProviderEarnings(message.Ukprn);
                if (earningEntities == null)
                {
                    return new GetProviderEarningsQueryResponse
                    {
                        IsValid = true,
                        Items = new PeriodEarning[0]
                    };
                }

                var periodEarnings = new List<PeriodEarning>();
                foreach (var entity in earningEntities)
                {
                    periodEarnings.AddRange(GetEarningsForPeriod(entity, message.Period1Month, message.Period1Year, message.AcademicYear));
                }

                return new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = periodEarnings.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetProviderEarningsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        private PeriodEarning[] GetEarningsForPeriod(EarningEntity entity, int period1Month, int period1Year, string academicYear)
        {
            var periodEarnings = new List<PeriodEarning>();

            var month = period1Month + entity.Period - 1;
            var year = period1Year;
            if (month > 12)
            {
                month -= 12;
                year++;
            }

            if (entity.PriceEpisodeOnProgPayment != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeOnProgPayment,
                    Type = TransactionType.Learning,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            if (entity.PriceEpisodeCompletionPayment != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeCompletionPayment,
                    Type = TransactionType.Completion,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            if (entity.PriceEpisodeBalancePayment != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeBalancePayment,
                    Type = TransactionType.Balancing,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            if (entity.PriceEpisodeFirstEmp1618Pay != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeFirstEmp1618Pay,
                    Type = TransactionType.First16To18EmployerIncentive,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            if (entity.PriceEpisodeFirstProv1618Pay != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeFirstProv1618Pay,
                    Type = TransactionType.First16To18ProviderIncentive,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            if (entity.PriceEpisodeSecondEmp1618Pay != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeSecondEmp1618Pay,
                    Type = TransactionType.Second16To18EmployerIncentive,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            if (entity.PriceEpisodeSecondProv1618Pay != 0)
            {
                periodEarnings.Add(new PeriodEarning
                {
                    CommitmentId = entity.CommitmentId,
                    CommitmentVersionId = entity.CommitmentVersionId,
                    AccountId = entity.AccountId,
                    AccountVersionId = entity.AccountVersionId,
                    Uln = entity.Uln,
                    Ukprn = entity.Ukprn,
                    LearnerReferenceNumber = entity.LearnerRefNumber,
                    AimSequenceNumber = entity.AimSequenceNumber,
                    CollectionPeriodNumber = entity.Period,
                    CollectionAcademicYear = academicYear,
                    CalendarMonth = month,
                    CalendarYear = year,
                    EarnedValue = entity.PriceEpisodeSecondProv1618Pay,
                    Type = TransactionType.Second16To18ProviderIncentive,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier
                });
            }

            return periodEarnings.ToArray();
        }
    }
}