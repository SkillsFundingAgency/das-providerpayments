using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.ParallelFramework
{
    [Binding]
    public sealed class ParallelisationHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        public ParallelContext ParallelContext { get; }

        public ParallelisationHooks(ParallelContext parallelContext)
        {
            ParallelContext = parallelContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var scenarioName = TestContext.CurrentContext.Test.Name;
            ParallelContext.Scenarios.Add(new Scenario
            {
                Id = ParallelContext.GetNextScenarioId(),
                Name = scenarioName
            });
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
