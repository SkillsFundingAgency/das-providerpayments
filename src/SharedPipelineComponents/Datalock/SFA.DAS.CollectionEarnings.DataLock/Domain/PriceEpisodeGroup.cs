using System;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    class PriceEpisodeGroup
    {
        public PriceEpisodeGroup(RawEarning earning)
        {
            PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier;
            ProgrammeType = earning.ProgrammeType;
            FrameworkCode = earning.FrameworkCode;
            StandardCode = earning.StandardCode;
            PathwayCode = earning.PathwayCode;
            AgreedPrice = earning.AgreedPrice;
            EpisodeStartDate = earning.EpisodeStartDate ?? new DateTime(9999, 01, 01); // Should never be null, but don't want to change all the references
        }

        public string PriceEpisodeIdentifier { get; set; }
        public int ProgrammeType { get; set; }
        public int FrameworkCode { get; set; }
        public int StandardCode { get; set; }
        public int PathwayCode { get; set; }
        public decimal AgreedPrice { get; set; }
        public DateTime EpisodeStartDate { get; set; }
    }
}
