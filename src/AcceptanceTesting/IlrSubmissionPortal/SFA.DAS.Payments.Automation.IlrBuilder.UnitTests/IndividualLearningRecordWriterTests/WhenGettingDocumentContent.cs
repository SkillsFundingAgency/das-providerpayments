using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace SFA.DAS.Payments.Automation.IlrBuilder.UnitTests.IndividualLearningRecordWriterTests
{
    public class WhenGettingDocumentContent
    {
        private static readonly string[] ElementsToExcludeFromValueCheck = {
            "\\Message\\Header\\Source\\DateTime"
        };

        public static readonly object[] TestIlrs = {
            new object[] {CommonIndividualLearningRecords.SingleDasLearnerOnAStandard(), Properties.Resources.SingleDasLearnerOnStandard}
        };

        [TestCaseSource("TestIlrs")]
        public void ThenItShouldReturnCorrectXmlString(IndividualLearningRecord ilr, string expctedXml)
        {
            // Arrange
            var writer = new IndividualLearningRecordWriter("1617");

            // Act
            var actualXml = writer.GetDocumentContent(ilr);

            // Assert
            AssertXmlMatches(expctedXml, actualXml);
        }


        private void AssertXmlMatches(string exptectedXml, string actualXml)
        {
            var expected = XDocument.Parse(exptectedXml);
            var actual = XDocument.Parse(actualXml);

            AssertXmlElementsMatch("", expected.Elements().ToArray(), actual.Elements().ToArray());
        }
        private void AssertXmlElementsMatch(string parentPath, XElement[] expectedElements, XElement[] actualElements)
        {
            if (expectedElements.Length != actualElements.Length)
            {
                throw new Exception($"Expected '{parentPath}' to have {expectedElements.Length} elements but has {actualElements.Length}");
            }

            for (var i = 0; i < expectedElements.Length; i++)
            {
                var expected = expectedElements[i];
                var actual = actualElements[i];

                if (expected.Name.LocalName != actual.Name.LocalName)
                {
                    throw new Exception($"Expected element {i} of '{parentPath}' to be {expected.Name.LocalName} but is {actual.Name.LocalName}");
                }

                var path = $"{parentPath}\\{expected.Name.LocalName}";
                if (expected.HasElements)
                {
                    AssertXmlElementsMatch(path, expected.Elements().ToArray(), actual.Elements().ToArray());
                }
                else if (!expected.IsEmpty)
                {
                    AssertElementValuesMatch(path, expected.Value, actual.Value);
                }
            }
        }
        private void AssertElementValuesMatch(string path, string expectedValue, string actualValue)
        {
            if (ElementsToExcludeFromValueCheck.Any(x => x == path))
            {
                return;
            }

            if (expectedValue != actualValue)
            {
                throw new Exception($"Expected '{path}' to have value '{expectedValue}' but has '{actualValue}'");
            }
        }
    }
}
