using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery
{
    public class GetAllLearningDeliveriesToProcessQueryHandler : IRequestHandler<GetAllLearningDeliveriesToProcessQueryRequest, GetAllLearningDeliveriesToProcessQueryResponse>
    {
        private readonly ILearningDeliveryToProcessRepository _learningDeliveryToProcessRepository;
        private readonly IFinancialRecordRepository _financialRecordRepository;

        public GetAllLearningDeliveriesToProcessQueryHandler(ILearningDeliveryToProcessRepository learningDeliveryToProcessRepository, IFinancialRecordRepository financialRecordRepository)
        {
            _learningDeliveryToProcessRepository = learningDeliveryToProcessRepository;
            _financialRecordRepository = financialRecordRepository;
        }

        public GetAllLearningDeliveriesToProcessQueryResponse Handle(GetAllLearningDeliveriesToProcessQueryRequest message)
        {
            try
            {
                var learningDeliveriesToProcess = _learningDeliveryToProcessRepository.GetAllLearningDeliveriesToProcess();

                return new GetAllLearningDeliveriesToProcessQueryResponse
                {
                    IsValid = true,
                    Items = learningDeliveriesToProcess == null 
                        ? null
                        : learningDeliveriesToProcess.Select(ld =>
                        {
                            var financialRecords = _financialRecordRepository.GetLearningDeliveryFinancialRecords(ld.LearnRefNumber, ld.AimSeqNumber);

                            return new LearningDelivery
                            {
                                Ukprn = ld.Ukprn,
                                LearnerReferenceNumber = ld.LearnRefNumber,
                                Uln = ld.Uln,
                                NiNumber = ld.NiNumber,
                                AimSequenceNumber = ld.AimSeqNumber,
                                StandardCode = ld.StandardCode,
                                ProgrammeType = ld.ProgrammeType,
                                FrameworkCode = ld.FrameworkCode,
                                PathwayCode = ld.PathwayCode,
                                LearningStartDate = ld.LearnStartDate,
                                OriginalLearningStartDate = ld.OrigLearnStartDate,
                                LearningPlannedEndDate = ld.LearnPlanEndDate,
                                LearningActualEndDate = ld.LearnActEndDate,
                                CompletionStatus = ld.CompletionStatus,
                                PriceEpisodes = GetPriceEpisodes(ld, financialRecords)
                            };
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetAllLearningDeliveriesToProcessQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

        private PriceEpisode[] GetPriceEpisodes(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, FinancialRecordEntity[] financialRecords)
        {
            var priceEpisodes = new List<PriceEpisode>();

            PriceEpisode currentPriceEpisode = null;

            foreach (var financialRecord in financialRecords)
            {
                var type = GetPriceEpisodeType(financialRecord);

                if (currentPriceEpisode == null)
                {
                    InitializeNewPriceEpisode(ref currentPriceEpisode, learningDelivery, financialRecord);

                    continue;
                }

                if (currentPriceEpisode.Type != type)
                {
                    ClosePriceEpisode(ref currentPriceEpisode, financialRecord.FinDate.AddDays(-1));
                    priceEpisodes.Add(currentPriceEpisode);
                    InitializeNewPriceEpisode(ref currentPriceEpisode, learningDelivery, financialRecord);

                    continue;
                }

                if (currentPriceEpisode.StartDate < financialRecord.FinDate)
                {
                    ClosePriceEpisode(ref currentPriceEpisode, financialRecord.FinDate.AddDays(-1));
                    priceEpisodes.Add(currentPriceEpisode);
                    InitializeNewPriceEpisode(ref currentPriceEpisode, learningDelivery, financialRecord);

                    continue;
                }

                currentPriceEpisode.NegotiatedPrice += financialRecord.FinAmount;
                currentPriceEpisode.SetTnp(financialRecord.FinCode, financialRecord.FinAmount);
            }

            if (currentPriceEpisode != null)
            {
                priceEpisodes.Add(currentPriceEpisode);
            }

            return priceEpisodes.ToArray();
        }

        private void InitializeNewPriceEpisode(ref PriceEpisode priceEpisode, Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, FinancialRecordEntity financialRecord)
        {
            priceEpisode = new PriceEpisode
            {
                Id = GetPriceEpisodeId(learningDelivery, financialRecord),
                StartDate = financialRecord.FinDate,
                NegotiatedPrice = financialRecord.FinAmount,
                Type = GetPriceEpisodeType(financialRecord),
                LastEpisode = true
            };

            priceEpisode.SetTnp(financialRecord.FinCode, financialRecord.FinAmount);
        }

        private void ClosePriceEpisode(ref PriceEpisode priceEpisode, DateTime endDate)
        {
            priceEpisode.EndDate = endDate;
            priceEpisode.LastEpisode = false;
        }

        private PriceEpisodeType GetPriceEpisodeType(FinancialRecordEntity financialRecord)
        {
            switch (financialRecord.FinCode)
            {
                case 1:
                case 2:
                    return PriceEpisodeType.Initial;
                case 3:
                case 4:
                    return PriceEpisodeType.Residual;
                default:
                    throw new ArgumentException($"Invalid TBFinCode value {financialRecord.FinCode}");
            }
        }

        private string GetPriceEpisodeId(Infrastructure.Data.Entities.LearningDeliveryToProcess learningDelivery, FinancialRecordEntity financialRecord)
        {
            return learningDelivery.StandardCode.HasValue
                ? learningDelivery.ProgrammeType + "-" + learningDelivery.StandardCode + "-" + financialRecord.FinDate.ToString("dd/MM/yyyy")
                : learningDelivery.ProgrammeType + "-" + learningDelivery.FrameworkCode + "-" + learningDelivery.PathwayCode + "-" + financialRecord.FinDate.ToString("dd/MM/yyyy");
        }
    }
}