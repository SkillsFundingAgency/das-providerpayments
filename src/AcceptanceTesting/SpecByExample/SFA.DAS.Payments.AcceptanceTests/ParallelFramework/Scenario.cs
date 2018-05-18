using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.AcceptanceTests.ParallelFramework
{
    public class Scenario
    {
        public Scenario()
        {
            Expectations = new List<IExpectation>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public List<IExpectation> Expectations { get; set; }
    }

    public interface IExpectation
    {
        List<string> AssertAndGetFailures();
    }

    public class ParallelContext
    {
        public ParallelContext()
        {
            _currentScenarioId = 1000;
        }

        public List<Scenario> Scenarios { get; set; }

        private long _currentScenarioId;

        public long GetNextScenarioId()
        {
            _currentScenarioId++;
            return _currentScenarioId;
        }
    }


}
