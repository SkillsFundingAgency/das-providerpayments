﻿namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class MatcherFactory
    {
        public static IMatcher CreateMatcher()
        {
            return new UlnMatchHandler(
                new UkprnMatchHandler(
                    new StandardMatchHandler(
                        new FrameworkMatchHandler(
                            new PathwayMatchHandler(
                                new ProgrammeMatchHandler(
                                    new PriceMatchHandler(
                                        new StartDateMatcher(
                                            new WithdrawnCommitmentMatcher(
                                                new PausedCommitmentMatcher(null))))))))));
        }
    }
}
