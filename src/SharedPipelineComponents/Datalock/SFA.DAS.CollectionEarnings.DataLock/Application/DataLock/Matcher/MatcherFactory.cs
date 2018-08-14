namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MatcherFactory
    {
        public static IMatcher CreateMatcher()
        {
            return new UlnMatchHandler(
                new StartDateMatcher(
                    new UkprnMatchHandler(
                        new StandardMatchHandler(
                            new FrameworkMatchHandler(
                                new PathwayMatchHandler(
                                    new ProgrammeMatchHandler(
                                        new PriceMatchHandler(null))))))));
        }
    }
}
