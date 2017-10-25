namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class Specification
    {
        public string Name { get; set; }
        public SpecificationArrangement Arrangement { get; set; } = new SpecificationArrangement();
        public SpecificationExpectations Expectations { get; set; } = new SpecificationExpectations();
    }
}
