namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class StepParserAbility
    {
        public StepParserAbility(string keyword, string descriptionPattern)
        {
            Keyword = keyword;
            DescriptionPattern = descriptionPattern;
        }
        public string Keyword { get; }
        public string DescriptionPattern { get; }
    }
}
