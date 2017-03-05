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

         
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Learning, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Completion, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Balancing, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.First16To18EmployerIncentive, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.First16To18ProviderIncentive, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Second16To18EmployerIncentive, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Second16To18ProviderIncentive, academicYear, month, year);

            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Balancing16To18FrameworkUplift, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.Completion16To18FrameworkUplift, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.OnProgramme16To18FrameworkUplift, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.FirstDisadvantagePayment, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.SecondDisadvantagePayment, academicYear, month, year);

            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.OnProgrammeMathsAndEnglish, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.BalancingMathsAndEnglish, academicYear, month, year);
            AddEarningForPeriodAndPaymentTypeIfAvailable(periodEarnings, entity, TransactionType.LearningSupport, academicYear, month, year);


            return periodEarnings.ToArray();
        }

        private void AddEarningForPeriodAndPaymentTypeIfAvailable(List<PeriodEarning> earnings, EarningEntity entity, TransactionType earningType, string academicYear, int month, int year)
        {
            decimal amount;

            switch (earningType)
            {
                case TransactionType.Learning:
                    amount = entity.PriceEpisodeOnProgPayment;
                    break;
                case TransactionType.Completion:
                    amount = entity.PriceEpisodeCompletionPayment;
                    break;
                case TransactionType.Balancing:
                    amount = entity.PriceEpisodeBalancePayment;
                    break;
                case TransactionType.First16To18EmployerIncentive:
                    amount = entity.PriceEpisodeFirstEmp1618Pay;
                    break;
                case TransactionType.First16To18ProviderIncentive:
                    amount = entity.PriceEpisodeFirstProv1618Pay;
                    break;
                case TransactionType.Second16To18EmployerIncentive:
                    amount = entity.PriceEpisodeSecondEmp1618Pay;
                    break;
                case TransactionType.Second16To18ProviderIncentive:
                    amount = entity.PriceEpisodeSecondProv1618Pay;
                    break;
                case TransactionType.OnProgramme16To18FrameworkUplift:
                    amount = entity.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment;
                    break;
                case TransactionType.Completion16To18FrameworkUplift:
                    amount = entity.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment;
                    break;
                case TransactionType.Balancing16To18FrameworkUplift:
                    amount = entity.PriceEpisodeApplic1618FrameworkUpliftBalancing;
                    break;
                case TransactionType.FirstDisadvantagePayment:
                    amount = entity.PriceEpisodeFirstDisadvantagePayment;
                    break;
                case TransactionType.SecondDisadvantagePayment:
                    amount = entity.PriceEpisodeSecondDisadvantagePayment;
                    break;
                case TransactionType.OnProgrammeMathsAndEnglish:
                    amount = entity.MathsAndEnglishOnProgPayment;
                    break;
                case TransactionType.BalancingMathsAndEnglish:
                    amount = entity.MathsAndEnglishBalancePayment;
                    break;
                case TransactionType.LearningSupport:
                    amount = entity.LearningSupportPayment;
                    break;
                default:
                    throw new ArgumentException($"Invalid transaction type of {earningType} found.");
            }

            if (amount != 0)
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
                    EarnedValue = amount,
                    Type = earningType,
                    StandardCode = entity.StandardCode,
                    FrameworkCode = entity.FrameworkCode,
                    ProgrammeType = entity.ProgrammeType,
                    PathwayCode = entity.PathwayCode,
                    ApprenticeshipContractType = entity.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = entity.PriceEpisodeIdentifier,
                    SfaContributionPercentage = entity.PriceEpisodeSfaContribPct,
                    FundingLineType = entity.PriceEpisodeFundLineType,
                    UseLevyBalance = entity.PriceEpisodeLevyNonPayInd.HasValue && entity.PriceEpisodeLevyNonPayInd.Value == 1 ? false : true
                });
            }
        }
    }
}