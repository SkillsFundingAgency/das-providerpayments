using System;
using System.Collections.Generic;
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

        public int Id { get; set; }
        public string Name { get; set; }
        public List<IExpectation> Expectations { get; set; }
    }

    public interface IExpectation
    {
        List<string> AssertAndGetFailures();
    }
}
