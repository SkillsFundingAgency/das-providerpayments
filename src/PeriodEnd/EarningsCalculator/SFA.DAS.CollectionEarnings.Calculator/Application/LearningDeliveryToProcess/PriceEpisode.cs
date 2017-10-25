using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess
{
    public class PriceEpisode
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int NegotiatedPrice { get; set; }
        public int? Tnp1 { get; set; }
        public int? Tnp2 { get; set; }
        public int? Tnp3 { get; set; }
        public int? Tnp4 { get; set; }
        public PriceEpisodeType Type { get; set; }
        public bool LastEpisode { get; set; }

        public void SetTnp(int finCode, int finAmount)
        {
            switch (finCode)
            {
                case 1:
                    Tnp1 = finAmount;
                    break;
                case 2:
                    Tnp2 = finAmount;
                    break;
                case 3:
                    Tnp3 = finAmount;
                    break;
                case 4:
                    Tnp4 = finAmount;
                    break;
                default:
                    throw new ArgumentException($"Invalid FinCode value of {finCode}");
            }
        }
    }
}