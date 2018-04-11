using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;
using System.Linq;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryHandler : IRequestHandler<GetProviderEarningsQueryRequest, GetProviderEarningsQueryResponse>
    {
        private readonly IEarningRepository _earningRepository;
        private readonly ICollectionPeriodRepository _collectionPeriodRepository;

        public GetProviderEarningsQueryHandler(IEarningRepository earningRepository,ICollectionPeriodRepository collectionPeriodRepository)
        {
            _earningRepository = earningRepository;
            _collectionPeriodRepository = collectionPeriodRepository;
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

                var collectionPeriods =_collectionPeriodRepository.GetAllCollectionPeriods();
                if (collectionPeriods == null)
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
                    periodEarnings.AddRange(GetEarningsForPeriod(entity, collectionPeriods, message.AcademicYear));
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

        private PeriodEarning[] GetEarningsForPeriod(EarningEntity entity, List<CollectionPeriodEntity> collectionPeriods, string academicYear)
        {
            var periodEarnings = new List<PeriodEarning>();

            var collectionPeriod = collectionPeriods.Single(x => x.PeriodId == entity.Period);

            var year = collectionPeriod.Year;
            var month = collectionPeriod.Month;
            

            AddEarningForPeriod(periodEarnings, entity, academicYear, month, year);

            return periodEarnings.ToArray();
        }

        private void AddEarningForPeriod(List<PeriodEarning> earnings, EarningEntity entity, string academicYear, int month, int year)
        {
            int contractTypeCode;

           

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
                ApprenticeshipContractTypeCode = int.TryParse(entity.ApprenticeshipContractTypeCode, out contractTypeCode) ? (int?)contractTypeCode : null,
                ApprenticeshipContractTypeStartDate = entity.ApprenticeshipContractTypeStartDate,
                ApprenticeshipContractTypeEndDate = entity.ApprenticeshipContractTypeEndDate,
                PriceEpisodeEndDate = entity.PriceEpisodeEndDate,
                PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier,
                SfaContributionPercentage = entity.PriceEpisodeSfaContribPct,
                FundingLineType = entity.PriceEpisodeFundLineType,
                UseLevyBalance = !entity.PriceEpisodeLevyNonPayInd.HasValue || entity.PriceEpisodeLevyNonPayInd.Value != 1,
                IsSuccess = entity.IsSuccess,
                Payable = entity.Payable,
                LearnAimRef =entity.LearnAimRef,
                LearningStartDate=entity.LearningStartDate,
                IsSmallEmployer = entity.IsSmallEmployer,
                IsOnEHCPlan = entity.IsOnEHCPlan,
                IsCareLeaver = entity.IsCareLeaver
            });

        }
    }
}