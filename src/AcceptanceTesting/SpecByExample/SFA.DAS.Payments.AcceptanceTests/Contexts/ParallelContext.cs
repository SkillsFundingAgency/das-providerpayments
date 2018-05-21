using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SFA.DAS.Payments.AcceptanceTests.ParallelFramework
{
    public class ParallelContext
    {
        public ParallelContext()
        {
            _currentScenarioId = 1000;
            Scenarios = new List<Scenario>();
        }

        public List<Scenario> Scenarios { get; set; }

        private int _currentScenarioId;

        public int GetNextScenarioId()
        {
            _currentScenarioId++;
            return _currentScenarioId;
        }

        public Scenario GetScenarioByName(string scenarioName)
        {
            return Scenarios.Single(x => x.Name == scenarioName);
        }

        public Scenario GetCurrentScenario()
        {
            return GetScenarioByName(TestContext.CurrentContext.Test.Name);
        }
    }
}