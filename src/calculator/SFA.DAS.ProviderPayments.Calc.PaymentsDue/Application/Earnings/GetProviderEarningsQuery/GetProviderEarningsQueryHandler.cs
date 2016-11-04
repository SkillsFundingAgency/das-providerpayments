using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

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
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 1, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 2, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 3, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 4, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 5, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 6, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 7, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 8, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 9, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 10, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 11, message.Period1Month, message.Period1Year, message.AcademicYear));
                    periodEarnings.AddRange(GetEarningForPeriod(entity, 12, message.Period1Month, message.Period1Year, message.AcademicYear));
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

        private PeriodEarning[] GetEarningForPeriod(EarningEntity entity, int periodNumber, int period1Month, int period1Year, string academicYear)
        {
            var month = period1Month + periodNumber - 1;
            var year = period1Year;
            if (month > 12)
            {
                month -= 12;
                year++;
            }
            decimal value;
            switch (periodNumber)
            {
                case 1:
                    value = entity.Period1;
                    break;
                case 2:
                    value = entity.Period2;
                    break;
                case 3:
                    value = entity.Period3;
                    break;
                case 4:
                    value = entity.Period4;
                    break;
                case 5:
                    value = entity.Period5;
                    break;
                case 6:
                    value = entity.Period6;
                    break;
                case 7:
                    value = entity.Period7;
                    break;
                case 8:
                    value = entity.Period8;
                    break;
                case 9:
                    value = entity.Period9;
                    break;
                case 10:
                    value = entity.Period10;
                    break;
                case 11:
                    value = entity.Period11;
                    break;
                case 12:
                    value = entity.Period12;
                    break;
                default:
                    throw new IndexOutOfRangeException($"Period number must be between 1 and 12 (periodNumber={periodNumber})");
            }

            if (value == 0)
            {
                return new PeriodEarning[0];
            }


            return new[] {new PeriodEarning
            {
                CommitmentId = entity.CommitmentId,
                CommitmentVersionId = entity.CommitmentVersionId,
                AccountId = entity.AccountId,
                AccountVersionId = entity.AccountVersionId,
                Uln = entity.Uln,
                Ukprn = entity.Ukprn,
                LearnerReferenceNumber = entity.LearnerRefNumber,
                AimSequenceNumber = entity.AimSequenceNumber,
                CollectionPeriodNumber = periodNumber,
                CollectionAcademicYear = academicYear,
                CalendarMonth = month,
                CalendarYear = year,
                EarnedValue = value,
                Type = TranslateEarningTypeToTransactionType(entity.EarningType)
            }};
        }

        private TransactionType TranslateEarningTypeToTransactionType(string earningType)
        {
            switch (earningType)
            {
                case EarningTypes.Learning:
                    return TransactionType.Learning;
                case EarningTypes.Completion:
                    return TransactionType.Completion;
                case EarningTypes.Balancing:
                    return TransactionType.Balancing;
                default:
                    throw new InvalidEarningTypeException(earningType);
            }
        }
    }
}