﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.DAS.Payments.AcceptanceTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Commitment effective dates apply correctly in data collections processing")]
    [NUnit.Framework.CategoryAttribute("CommitmentsEffectiveDate")]
    public partial class CommitmentEffectiveDatesApplyCorrectlyInDataCollectionsProcessingFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "commitment_effectiveness.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-GB"), "Commitment effective dates apply correctly in data collections processing", null, ProgrammingLanguage.CSharp, new string[] {
                        "CommitmentsEffectiveDate"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
    #line 5
        testRunner.Given("the apprenticeship funding band maximum for each learner is 17000", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
        testRunner.And("levy balance > agreed price for all months", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Change in price month 2")]
        public virtual void ChangeInPriceMonth2()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Change in price month 2", ((string[])(null)));
#line 8
 this.ScenarioSetup(scenarioInfo);
#line 4
    this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table313 = new TechTalk.SpecFlow.Table(new string[] {
                        "commitment Id",
                        "version Id",
                        "Employer",
                        "Provider",
                        "ULN",
                        "start date",
                        "end date",
                        "agreed price",
                        "effective from",
                        "effective to"});
            table313.AddRow(new string[] {
                        "1",
                        "1-001",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "7500",
                        "01/05/2017",
                        "31/05/2017"});
            table313.AddRow(new string[] {
                        "1",
                        "2-001",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/01/2017",
                        "01/05/2018",
                        "15000",
                        "01/06/2017",
                        ""});
#line 9
  testRunner.Given("the following commitments exist:", ((string)(null)), table313, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table314 = new TechTalk.SpecFlow.Table(new string[] {
                        "ULN",
                        "learner type",
                        "start date",
                        "planned end date",
                        "completion status",
                        "Total training price 1",
                        "Total training price 1 effective date",
                        "Total training price 2",
                        "Total training price 2 effective date"});
            table314.AddRow(new string[] {
                        "learner a",
                        "programme only DAS",
                        "12/05/2017",
                        "20/05/2018",
                        "continuing",
                        "7500",
                        "01/05/2017",
                        "15000",
                        "01/06/2017"});
#line 14
  testRunner.When("an ILR file is submitted with the following data:", ((string)(null)), table314, "When ");
#line hidden
            TechTalk.SpecFlow.Table table315 = new TechTalk.SpecFlow.Table(new string[] {
                        "Payment type",
                        "05/17",
                        "06/17",
                        "07/17"});
            table315.AddRow(new string[] {
                        "On-program",
                        "commitment 1 v1-001",
                        "commitment 1 v2-001",
                        "commitment 1 v2-001"});
#line 18
  testRunner.Then("the data lock status will be as follows:", ((string)(null)), table315, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table316 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17"});
            table316.AddRow(new string[] {
                        "Provider Earned Total",
                        "500",
                        "1045.45",
                        "1045.45"});
            table316.AddRow(new string[] {
                        "Provider Earned from SFA",
                        "500",
                        "1045.45",
                        "1045.45"});
            table316.AddRow(new string[] {
                        "Provider Paid by SFA",
                        "0",
                        "500",
                        "1045.45"});
            table316.AddRow(new string[] {
                        "Levy account debited",
                        "0",
                        "500",
                        "1045.45"});
            table316.AddRow(new string[] {
                        "SFA Levy employer budget",
                        "500",
                        "1045.45",
                        "1045.45"});
            table316.AddRow(new string[] {
                        "SFA Levy co-funding budget",
                        "0",
                        "0",
                        "0"});
#line 21
        testRunner.And("the provider earnings and payments break down as follows:", ((string)(null)), table316, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Change in price month 2 with priority change after")]
        public virtual void ChangeInPriceMonth2WithPriorityChangeAfter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Change in price month 2 with priority change after", ((string[])(null)));
#line 32
 this.ScenarioSetup(scenarioInfo);
#line 4
    this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table317 = new TechTalk.SpecFlow.Table(new string[] {
                        "commitment Id",
                        "version Id",
                        "priority",
                        "Employer",
                        "Provider",
                        "ULN",
                        "start date",
                        "end date",
                        "agreed price",
                        "effective from",
                        "effective to"});
            table317.AddRow(new string[] {
                        "1",
                        "1-001",
                        "1",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "7500",
                        "01/05/2017",
                        "31/05/2017"});
            table317.AddRow(new string[] {
                        "1",
                        "2-001",
                        "1",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "15000",
                        "01/06/2017",
                        "13/07/2017"});
            table317.AddRow(new string[] {
                        "1",
                        "3-001",
                        "2",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "15000",
                        "14/07/2017",
                        ""});
#line 33
  testRunner.Given("the following commitments exist:", ((string)(null)), table317, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table318 = new TechTalk.SpecFlow.Table(new string[] {
                        "ULN",
                        "learner type",
                        "start date",
                        "planned end date",
                        "completion status",
                        "Total training price 1",
                        "Total training price 1 effective date",
                        "Total training price 2",
                        "Total training price 2 effective date"});
            table318.AddRow(new string[] {
                        "learner a",
                        "programme only DAS",
                        "12/05/2017",
                        "20/05/2018",
                        "continuing",
                        "7500",
                        "01/05/2017",
                        "15000",
                        "01/06/2017"});
#line 39
  testRunner.When("an ILR file is submitted with the following data:", ((string)(null)), table318, "When ");
#line hidden
            TechTalk.SpecFlow.Table table319 = new TechTalk.SpecFlow.Table(new string[] {
                        "Payment type",
                        "05/17",
                        "06/17",
                        "07/17"});
            table319.AddRow(new string[] {
                        "On-program",
                        "commitment 1 v1-001",
                        "commitment 1 v2-001",
                        "commitment 1 v3-001"});
#line 43
  testRunner.Then("the data lock status will be as follows:", ((string)(null)), table319, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table320 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17"});
            table320.AddRow(new string[] {
                        "Provider Earned Total",
                        "500",
                        "1045.45",
                        "1045.45"});
            table320.AddRow(new string[] {
                        "Provider Earned from SFA",
                        "500",
                        "1045.45",
                        "1045.45"});
            table320.AddRow(new string[] {
                        "Provider Paid by SFA",
                        "0",
                        "500",
                        "1045.45"});
            table320.AddRow(new string[] {
                        "Levy account debited",
                        "0",
                        "500",
                        "1045.45"});
            table320.AddRow(new string[] {
                        "SFA Levy employer budget",
                        "500",
                        "1045.45",
                        "1045.45"});
            table320.AddRow(new string[] {
                        "SFA Levy co-funding budget",
                        "0",
                        "0",
                        "0"});
#line 46
        testRunner.Then("the provider earnings and payments break down as follows:", ((string)(null)), table320, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Multiple versions created and should match correct version for each period")]
        public virtual void MultipleVersionsCreatedAndShouldMatchCorrectVersionForEachPeriod()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Multiple versions created and should match correct version for each period", ((string[])(null)));
#line 57
 this.ScenarioSetup(scenarioInfo);
#line 4
    this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table321 = new TechTalk.SpecFlow.Table(new string[] {
                        "commitment Id",
                        "version Id",
                        "priority",
                        "Employer",
                        "Provider",
                        "ULN",
                        "start date",
                        "end date",
                        "agreed price",
                        "effective from",
                        "effective to"});
            table321.AddRow(new string[] {
                        "1",
                        "1-001",
                        "1",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "7500",
                        "01/05/2017",
                        "27/06/2017"});
            table321.AddRow(new string[] {
                        "1",
                        "2-001",
                        "1",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "7500",
                        "28/06/2017",
                        "28/06/2017"});
            table321.AddRow(new string[] {
                        "1",
                        "3-001",
                        "2",
                        "employer 1",
                        "provider a",
                        "learner a",
                        "01/05/2017",
                        "01/05/2018",
                        "7500",
                        "29/06/2017",
                        ""});
#line 58
  testRunner.Given("the following commitments exist:", ((string)(null)), table321, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table322 = new TechTalk.SpecFlow.Table(new string[] {
                        "ULN",
                        "learner type",
                        "start date",
                        "planned end date",
                        "completion status",
                        "Total training price 1",
                        "Total training price 1 effective date"});
            table322.AddRow(new string[] {
                        "learner a",
                        "programme only DAS",
                        "12/05/2017",
                        "20/05/2018",
                        "continuing",
                        "7500",
                        "01/05/2017"});
#line 64
  testRunner.When("an ILR file is submitted with the following data:", ((string)(null)), table322, "When ");
#line hidden
            TechTalk.SpecFlow.Table table323 = new TechTalk.SpecFlow.Table(new string[] {
                        "Payment type",
                        "05/17",
                        "06/17",
                        "07/17"});
            table323.AddRow(new string[] {
                        "On-program",
                        "commitment 1 v1-001",
                        "commitment 1 v3-001",
                        "commitment 1 v3-001"});
#line 68
  testRunner.Then("the data lock status will be as follows:", ((string)(null)), table323, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table324 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17"});
            table324.AddRow(new string[] {
                        "Provider Earned Total",
                        "500",
                        "500",
                        "500"});
            table324.AddRow(new string[] {
                        "Provider Earned from SFA",
                        "500",
                        "500",
                        "500"});
            table324.AddRow(new string[] {
                        "Provider Paid by SFA",
                        "0",
                        "500",
                        "500"});
            table324.AddRow(new string[] {
                        "Levy account debited",
                        "0",
                        "500",
                        "500"});
            table324.AddRow(new string[] {
                        "SFA Levy employer budget",
                        "500",
                        "500",
                        "500"});
            table324.AddRow(new string[] {
                        "SFA Levy co-funding budget",
                        "0",
                        "0",
                        "0"});
#line 71
        testRunner.Then("the provider earnings and payments break down as follows:", ((string)(null)), table324, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
