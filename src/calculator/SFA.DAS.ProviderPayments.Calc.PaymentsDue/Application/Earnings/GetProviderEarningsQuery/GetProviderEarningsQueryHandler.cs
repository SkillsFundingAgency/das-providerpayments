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
            var academicStart = new DateTime(period1Year, period1Month, 1);

            if (academicStart > entity.PriceEpisodeEndDate)
            {
                return periodEarnings.ToArray();
            }

            var month = period1Month + entity.Period - 1;
            var year = period1Year;
            if (month > 12)
            {
                month -= 12;
                year++;
            }

            AddEarningForPeriod(periodEarnings, entity, academicYear, month, year);

            return periodEarnings.ToArray();
        }

        private void AddEarningForPeriod(List<PeriodEarning> earnings, EarningEntity entity, string academicYear, int month, int year)
        {
            earnings.Add(new PeriodEarning
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
                EarnedValue = entity.Amount,
                Type = (TransactionType)entity.TransactionType,
                StandardCode = entity.StandardCode,
                FrameworkCode = entity.FrameworkCode,
                ProgrammeType = entity.ProgrammeType,
                PathwayCode = entity.PathwayCode,
                ApprenticeshipContractType = entity.ApprenticeshipContractType,
                PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier,
                SfaContributionPercentage = entity.PriceEpisodeSfaContribPct,
                FundingLineType = entity.PriceEpisodeFundLineType,
                UseLevyBalance = entity.PriceEpisodeLevyNonPayInd.HasValue && entity.PriceEpisodeLevyNonPayInd.Value == 1 ? false : true,
                IsSuccess = entity.IsSuccess,
                Payable = entity.Payable,
                LearnAimRef =entity.LearnAimRef,
                LearningStartDate=entity.LearningStartDate
            });

        }
    }
}