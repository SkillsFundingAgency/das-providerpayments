using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand
{
    public class ProcessAddPriceEpisodesCommandHandler : IRequestHandler<ProcessAddPriceEpisodesCommandRequest, Unit>
    {
        private readonly IApprenticeshipPriceEpisodeRepository _repository;

        public ProcessAddPriceEpisodesCommandHandler(IApprenticeshipPriceEpisodeRepository repository)
        {
            _repository = repository;
        }

        public Unit Handle(ProcessAddPriceEpisodesCommandRequest message)
        {
            var entities = message.PriceEpisodes
                .Select(
                    pe =>
                        new ApprenticeshipPriceEpisodeEntity
                        {
                            LearnRefNumber = pe.LearnerReferenceNumber,
                            PriceEpisodeAimSeqNumber = pe.AimSequenceNumber,
                            EpisodeEffectiveTNPStartDate = pe.StartDate,
                            EpisodeStartDate = pe.StartDate,
                            PriceEpisodeIdentifier = pe.Id,
                            PriceEpisodeActualEndDate = pe.ActualEndDate,
                            PriceEpisodeCompleted = pe.Completed,
                            PriceEpisodeCompletionElement = pe.CompletionAmount,
                            PriceEpisodeInstalmentValue = pe.MonthlyAmount,
                            PriceEpisodePlannedEndDate = pe.PlannedEndDate,
                            PriceEpisodeTotalTNPPrice = pe.NegotiatedPrice,
                            TNP1 = pe.Tnp1,
                            TNP2 = pe.Tnp2,
                            TNP3 = pe.Tnp3,
                            TNP4 = pe.Tnp4
                        })
                .ToArray();

            _repository.AddApprenticeshipPriceEpisodes(entities);

            return Unit.Value;
        }
    }
}