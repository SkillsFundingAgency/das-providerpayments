# Provider Payments Service (BETA)   ![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/123/badge)

This solution represents the code base relating to apprenticeship payment to providers

## Running Locally

### Prerequisites

To run the solution locally you will need:
* Visual Studio 2015
* [Azure SDK 2.9 (for Visual Studio)](https://azure.microsoft.com/en-us/downloads/)

You should run Visual Studio as Administrator

Currently, the Resharper supported C# version is 6.0, which has been enforced within the $/src/SFA.DAS.PaymentComponents.sln.DotSettings file with the line:

<s:String x:Key="/Default/CodeInspection/CSharpLanguageProject/LanguageLevel/@EntryValue">CSharp60</s:String>