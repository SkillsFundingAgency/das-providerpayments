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
    [NUnit.Framework.DescriptionAttribute("Provider adjustments (EAS) payments")]
    [NUnit.Framework.CategoryAttribute("ProviderAdjustments")]
    public partial class ProviderAdjustmentsEASPaymentsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "provider_adjustments.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-GB"), "Provider adjustments (EAS) payments", null, ProgrammingLanguage.CSharp, new string[] {
                        "ProviderAdjustments"});
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Payments when current and previous provider adjustments exist")]
        public virtual void PaymentsWhenCurrentAndPreviousProviderAdjustmentsExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Payments when current and previous provider adjustments exist", ((string[])(null)));
#line 4
    this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table700 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "year to date total"});
            table700.AddRow(new string[] {
                        "16-18 levy additional provider payments: audit adjustments",
                        "143.52",
                        "13.59",
                        "157.11"});
            table700.AddRow(new string[] {
                        "16-18 non-levy additional provider payments: training authorised claims",
                        "17.57",
                        "11.89",
                        "29.46"});
            table700.AddRow(new string[] {
                        "Adult levy training: authorised claims",
                        "501.02",
                        "98.14",
                        "599.16"});
            table700.AddRow(new string[] {
                        "Adult non-levy additional employer payments: audit adjustments",
                        "305.25",
                        "5.23",
                        "310.48"});
#line 5
        testRunner.Given("that the previous EAS entries for a provider are as follows:", ((string)(null)), table700, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table701 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17",
                        "year to date total"});
            table701.AddRow(new string[] {
                        "16-18 levy additional provider payments: audit adjustments",
                        "195.17",
                        "4.98",
                        "63.42",
                        "263.57"});
            table701.AddRow(new string[] {
                        "16-18 non-levy additional provider payments: training authorised claims",
                        "17.57",
                        "2.89",
                        "2.45",
                        "22.91"});
            table701.AddRow(new string[] {
                        "Adult levy training: authorised claims",
                        "475.34",
                        "98.14",
                        "65.49",
                        "638.97"});
            table701.AddRow(new string[] {
                        "Adult levy additional provider payments: audit adjustments",
                        "0",
                        "18.65",
                        "1.63",
                        "20.28"});
            table701.AddRow(new string[] {
                        "Adult non-levy additional employer payments: audit adjustments",
                        "341.25",
                        "5.23",
                        "159.34",
                        "505.82"});
#line 11
        testRunner.When("the following EAS entries are submitted:", ((string)(null)), table701, "When ");
#line hidden
            TechTalk.SpecFlow.Table table702 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17",
                        "payments year to date"});
            table702.AddRow(new string[] {
                        "16-18 levy additional provider payments: audit adjustments",
                        "51.65",
                        "-8.61",
                        "63.42",
                        "106.46"});
            table702.AddRow(new string[] {
                        "16-18 non-levy additional provider payments: training authorised claims",
                        "0",
                        "-9.00",
                        "2.45",
                        "-6.55"});
            table702.AddRow(new string[] {
                        "Adult levy training: authorised claims",
                        "-25.68",
                        "0",
                        "65.49",
                        "39.81"});
            table702.AddRow(new string[] {
                        "Adult levy additional provider payments: audit adjustments",
                        "0",
                        "18.65",
                        "1.63",
                        "20.28"});
            table702.AddRow(new string[] {
                        "Adult non-levy additional employer payments: audit adjustments",
                        "36.00",
                        "0",
                        "159.34",
                        "195.34"});
#line 18
        testRunner.Then("the following adjustments will be generated:", ((string)(null)), table702, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Payments when only current provider adjustments exist")]
        public virtual void PaymentsWhenOnlyCurrentProviderAdjustmentsExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Payments when only current provider adjustments exist", ((string[])(null)));
#line 27
    this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table703 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "year to date total"});
            table703.AddRow(new string[] {
                        "16-18 levy additional provider payments: audit adjustments",
                        "0",
                        "0",
                        "0"});
            table703.AddRow(new string[] {
                        "16-18 non-levy additional provider payments: training authorised claims",
                        "0",
                        "0",
                        "0"});
            table703.AddRow(new string[] {
                        "Adult levy training: authorised claims",
                        "0",
                        "0",
                        "0"});
            table703.AddRow(new string[] {
                        "Adult non-levy additional employer payments: audit adjustments",
                        "0",
                        "0",
                        "0"});
#line 28
        testRunner.Given("that the previous EAS entries for a provider are as follows:", ((string)(null)), table703, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table704 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17",
                        "year to date total"});
            table704.AddRow(new string[] {
                        "16-18 levy additional provider payments: audit adjustments",
                        "195.17",
                        "4.98",
                        "63.42",
                        "263.57"});
            table704.AddRow(new string[] {
                        "16-18 non-levy additional provider payments: training authorised claims",
                        "17.57",
                        "2.89",
                        "2.45",
                        "22.91"});
            table704.AddRow(new string[] {
                        "Adult levy training: authorised claims",
                        "475.34",
                        "98.14",
                        "65.49",
                        "638.97"});
            table704.AddRow(new string[] {
                        "Adult levy additional provider payments: audit adjustments",
                        "0",
                        "18.65",
                        "1.63",
                        "20.28"});
            table704.AddRow(new string[] {
                        "Adult non-levy additional employer payments: audit adjustments",
                        "341.25",
                        "5.23",
                        "159.34",
                        "505.82"});
#line 34
        testRunner.When("the following EAS entries are submitted:", ((string)(null)), table704, "When ");
#line hidden
            TechTalk.SpecFlow.Table table705 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "05/17",
                        "06/17",
                        "07/17",
                        "year to date total"});
            table705.AddRow(new string[] {
                        "16-18 levy additional provider payments: audit adjustments",
                        "195.17",
                        "4.98",
                        "63.42",
                        "263.57"});
            table705.AddRow(new string[] {
                        "16-18 non-levy additional provider payments: training authorised claims",
                        "17.57",
                        "2.89",
                        "2.45",
                        "22.91"});
            table705.AddRow(new string[] {
                        "Adult levy training: authorised claims",
                        "475.34",
                        "98.14",
                        "65.49",
                        "638.97"});
            table705.AddRow(new string[] {
                        "Adult levy additional provider payments: audit adjustments",
                        "0",
                        "18.65",
                        "1.63",
                        "20.28"});
            table705.AddRow(new string[] {
                        "Adult non-levy additional employer payments: audit adjustments",
                        "341.25",
                        "5.23",
                        "159.34",
                        "505.82"});
#line 41
        testRunner.Then("the following adjustments will be generated:", ((string)(null)), table705, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
