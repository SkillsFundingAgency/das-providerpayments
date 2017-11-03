using System;
using Gherkin.Ast;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers
{
    public class SubmissionStepParser : StepParser
    {
        public SubmissionStepParser()
            : base(new StepParserAbility("When", "an ILR file is submitted with the following data"),
                   new StepParserAbility("When", "the providers submit the following ILR files"),
                   new StepParserAbility("When", "an ILR file is submitted on (.*) with the following data"),
                   new StepParserAbility("When", "an ILR file is submitted for the first time"))
            
        {
        }

        public override void Parse(Step step, Specification specification)
        {
            var table = step.Argument as DataTable;
            var learnerRecords = TableParser.ParseTable<LearnerRecordEntity>("Submission", table);
            foreach (var learnerRecord in learnerRecords)
            {
                if (!learnerRecord.StandardCode.HasValue && !learnerRecord.FrameworkCode.HasValue)
                {
                    learnerRecord.StandardCode = Defaults.StandardCode;
                }
                if (learnerRecord.AgreedPrice.HasValue && learnerRecord.TotalTrainingPrice == 0)
                {
                    learnerRecord.TotalTrainingPrice = learnerRecord.AgreedPrice.Value;
                }

                if (string.IsNullOrEmpty(learnerRecord.LearnerReferenceNumber) && 
                    !string.IsNullOrEmpty(learnerRecord.LearnerKey))
                {
                    learnerRecord.LearnerReferenceNumber = learnerRecord.LearnerKey;
                }
            }
            specification.Arrangement.LearnerRecords.AddRange(learnerRecords);
        }

        private class LearnerRecordEntity : LearnerRecord
        {
            [ColumnHeader("ULN")]
            [DefaultValue(Defaults.LearnerKey)]
            public override string LearnerKey { get; set; }

            [Optional]
            public override string LearnerReferenceNumber { get; set; }

            [DefaultValue(LearnerType.ProgrammeOnlyDas)]
            public override LearnerType LearnerType { get; set; }

            [ColumnHeader("Provider")]
            [DefaultValue(Defaults.ProviderKey)]
            public override string ProviderKey { get; set; }

            [DefaultValue(AimType.Programme)]
            public override AimType AimType { get; set; }

            [Optional]
            public override string AimRate { get; set; }

            [Optional]
            public override string HomePostcodeDeprivation { get; set; }

            [Optional]
            public override string EmploymentStatus { get; set; }

            [Optional]
            public override string EmploymentStatusApplies { get; set; }

            [Optional]
            public override string SmallEmployer { get; set; }

            [Optional]
            public override string LearnDelFam { get; set; }

            [ColumnHeader("Employer")]
            [DefaultValue(Defaults.EmployerKey)]
            public override string EmployerKey { get; set; }


            [Obsolete("Should use TotalTrainingPrice / TotalAssessmentPrice variants")] //TODO: Remove from spec and then can be removed from here
            [Optional]
            [ColumnHeader("AgreedPrice")]
            public virtual int? AgreedPrice {get;set;}

            [Optional]
            public override int TotalTrainingPrice1 { get; set; }

            [Optional]
            public override DateTime TotalTrainingPrice1EffectiveDate { get; set; }

            [Optional]
            public virtual int TotalTrainingPrice
            {
                get { return TotalTrainingPrice1; }
                set { TotalTrainingPrice1 = value; }
            }
            [Optional]
            public virtual DateTime TotalTrainingPriceEffectiveDate
            {
                get { return TotalTrainingPrice1EffectiveDate; }
                set { TotalTrainingPrice1EffectiveDate = value; }
            }
            public virtual int? TotalAssessmentPrice
            {
                get { return TotalAssessmentPrice1; }
                set { TotalAssessmentPrice1 = value; }
            }
            public virtual DateTime? TotalAssessmentPriceEffectiveDate
            {
                get { return TotalAssessmentPrice1EffectiveDate; }
                set { TotalAssessmentPrice1EffectiveDate = value; }
            }



            public override int? ResidualTrainingPrice1 { get; set; }

            public override DateTime? ResidualTrainingPrice1EffectiveDate { get; set; }

            public virtual int? ResidualTrainingPrice
            {
                get { return ResidualTrainingPrice1; }
                set { ResidualTrainingPrice1 = value; }
            }
            public virtual DateTime? ResidualTrainingPriceEffectiveDate
            {
                get { return ResidualTrainingPrice1EffectiveDate; }
                set { ResidualTrainingPrice1EffectiveDate = value; }
            }
            public virtual int? ResidualAssessmentPrice
            {
                get { return ResidualAssessmentPrice1; }
                set { ResidualAssessmentPrice1 = value; }
            }
            public virtual DateTime? ResidualAssessmentPriceEffectiveDate
            {
                get { return ResidualAssessmentPrice1EffectiveDate; }
                set { ResidualAssessmentPrice1EffectiveDate = value; }
            }

            [Optional]
            [DefaultValue(Defaults.AimSequenceNumber)]
            public override int AimSequenceNumber { get; set; }
        }
    }
}
