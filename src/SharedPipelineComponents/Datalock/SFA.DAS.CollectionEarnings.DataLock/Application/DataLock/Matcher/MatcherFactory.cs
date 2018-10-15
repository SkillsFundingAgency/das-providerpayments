namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MatcherFactory
    {
        public static IMatcher CreateMatcher()
        {
            return new UlnMatchHandler(
                new UkprnMatchHandler(
                    new StartDateMatcher(
                        new StandardMatchHandler(
                            new FrameworkMatchHandler(
                                new PathwayMatchHandler(
                                    new ProgrammeMatchHandler(
                                        new PriceMatchHandler(
                                            new WithdrawnCommitmentMatcher(
                                                new PausedCommitmentMatcher(null))))))))));
        }
    }
}
