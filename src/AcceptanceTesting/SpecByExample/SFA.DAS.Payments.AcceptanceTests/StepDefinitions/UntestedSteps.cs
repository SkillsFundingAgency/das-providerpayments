﻿using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class UntestedSteps
    {
        [Given("The learner is programme only DAS")]
        [Given("Two learners are programme only DAS")]
        public void GivenTheLearnerIsProgrammeOnlyDas()
        {
        }

        [Then(@"the following capping will apply to the price episodes:")]
        public void ThenTheFollowingCappingWillApplyToThePriceEpisodes(Table table)
        {
            //TODO
        }
    }
}
