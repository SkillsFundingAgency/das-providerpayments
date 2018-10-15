namespace SFA.DAS.CollectionEarnings.DataLock.Domain.Extensions
{
    public static class CommitmentExtensions
    {
        public static bool WithdrawnBeforeStart(this Commitment source)
        {
            if (source.WithdrawnOnDate < source.EffectiveFrom)
            {
                return true;
            }

            return false;
        }
    }
}
