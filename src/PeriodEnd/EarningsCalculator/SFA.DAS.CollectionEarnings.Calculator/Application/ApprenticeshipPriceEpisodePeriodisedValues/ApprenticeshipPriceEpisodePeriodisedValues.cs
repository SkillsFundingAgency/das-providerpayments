using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues
{
    public class ApprenticeshipPriceEpisodePeriodisedValues
    {
        public string PriceEpisodeId { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public DateTime PriceEpisodeStartDate { get; set; }

        public AttributeNames AttributeName { get; set; }

        public decimal Period1Amount { get; set; }
        public decimal Period2Amount { get; set; }
        public decimal Period3Amount { get; set; }
        public decimal Period4Amount { get; set; }
        public decimal Period5Amount { get; set; }
        public decimal Period6Amount { get; set; }
        public decimal Period7Amount { get; set; }
        public decimal Period8Amount { get; set; }
        public decimal Period9Amount { get; set; }
        public decimal Period10Amount { get; set; }
        public decimal Period11Amount { get; set; }
        public decimal Period12Amount { get; set; }

        public void SetPeriodValue(int period, decimal value)
        {
            switch (period)
            {
                case 1:
                    Period1Amount = value;
                    break;
                case 2:
                    Period2Amount = value;
                    break;
                case 3:
                    Period3Amount = value;
                    break;
                case 4:
                    Period4Amount = value;
                    break;
                case 5:
                    Period5Amount = value;
                    break;
                case 6:
                    Period6Amount = value;
                    break;
                case 7:
                    Period7Amount = value;
                    break;
                case 8:
                    Period8Amount = value;
                    break;
                case 9:
                    Period9Amount = value;
                    break;
                case 10:
                    Period10Amount = value;
                    break;
                case 11:
                    Period11Amount = value;
                    break;
                case 12:
                    Period12Amount = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected period value: {period}");
            }
        }

        public decimal GetPeriodValue(int period)
        {
            switch (period)
            {
                case 1:
                    return Period1Amount;
                case 2:
                    return Period2Amount;
                case 3:
                    return Period3Amount;
                case 4:
                    return Period4Amount;
                case 5:
                    return Period5Amount;
                case 6:
                    return Period6Amount;
                case 7:
                    return Period7Amount;
                case 8:
                    return Period8Amount;
                case 9:
                    return Period9Amount;
                case 10:
                    return Period10Amount;
                case 11:
                    return Period11Amount;
                case 12:
                    return Period12Amount;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected period value: {period}");
            }
        }
    }
}