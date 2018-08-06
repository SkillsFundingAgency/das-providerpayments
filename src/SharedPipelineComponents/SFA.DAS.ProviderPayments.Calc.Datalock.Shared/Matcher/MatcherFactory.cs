using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher.MatchingSteps;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher
{
    public class MatcherFactory
    {
        public static IMatcher CreateMatcher()
        {
            return  new UlnMatchHandler(
                            new StartDateMatcher(
                                new MultipleMatchHandler(
                                    new UkprnMatchHandler(
                                        new StandardMatchHandler(
                                            new FrameworkMatchHandler(
                                                new PathwayMatchHandler(
                                                    new ProgrammeMatchHandler(
                                                        new PriceMatchHandler(
                                                            new LevyPayerFlagMatcher(null))))))))));

          
        }
    }
}
