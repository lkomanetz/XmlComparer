using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlComparer.Contracts;
using System.Text;

namespace XmlComparer.Tests {

	[TestClass]
	public class XmlComparerTests {

		[TestMethod]
		public void WhenXmlIsEqualComparerSucceeds() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");

			Assert.IsTrue(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenAttributesAreDifferentComparerFails() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child test='2'><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child test='1'><grandchild>hello</grandchild></child></root>");

			Assert.IsFalse(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WithMultipleAttributesThatAreEqualComparerSucceeds() {
			string xml = @"
				<root>
					<parents>
						<parent id='1' name='John'>
							<child></child>
						</parent>
						<parent id='2' name='Mary'>
							<child></child>
							<child></child>
						</parent>
					</parents>
				</root>";

			Assert.IsTrue(new XmlComparer().AreEqual(xml, xml));
		}

		[TestMethod]
		public void WhenMultipleAttributesAreDifferentComparerFails() {
			string xmlA = @"
				<root>
					<parents>
						<parent id='1' name='John'>
							<child></child>
						</parent>
						<parent id='2' name='Mary'>
							<child></child>
							<child></child>
						</parent>
					</parents>
				</root>";

			string xmlB = @"
				<root>
					<parents>
						<parent id='1' name='Jake'>
							<child></child>
						</parent>
						<parent id='2' name='Jane'>
							<child></child>
							<child></child>
						</parent>
					</parents>
				</root>";

			XmlComparer comparer = new XmlComparer();
			comparer.AreEqual(xmlA, xmlB);
			Assert.IsFalse(comparer.Results.IsSuccessful);
		}

		[TestMethod]
		public void ComparisonResultsAreCorrect() {
			string xmlA = @"
				<root>
					<parents>
						<parent id='1' name='John'>
							<child></child>
						</parent>
						<parent id='2' name='Mary'>
							<child></child>
							<child></child>
						</parent>
					</parents>
				</root>";

			string xmlB = @"
				<root>
					<parents>
						<parent id='1' name='Jake'>
							<child></child>
						</parent>
						<parent id='2' name='Jane'>
							<child></child>
							<child></child>
						</parent>
					</parents>
				</root>";

			XmlComparer comparer = new XmlComparer();
			comparer.AreEqual(xmlA, xmlB);
			AssertResults(
				"child:Truechild:Truechild:Trueparent:Falseparent:Falseparents:Trueroot:True",
				comparer.Results
			);
		}

		[TestMethod]
		public void WithComplexStructureThatIsTheSameComparerSucceeds() {
			string xml = @"
				<root>
					<child test='1'>
					</child>
					<child test='2'>
						<grandchild>Hello</grandchild>
						<grandchild>Again</grandchild>
					</child>
				</root>";

			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(xml);
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(xml);

			Assert.IsTrue(new XmlComparer().AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WithComplexStructureThatAreDifferentComparerFails() {
			string xmlA = @"
				<root>
					<child test='1'>
					</child>
					<child test='2'>
						<grandchild>Hello</grandchild>
						<grandchild>Again</grandchild>
					</child>
				</root>";
			string xmlB = @"
				<root>
					<child test='1'>
					</child>
					<child test='3'>
						<grandchild>Hello</grandchild>
						<grandchild>Kid</grandchild>
					</child>
				</root>";

			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(xmlA);
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(xmlB);

			Assert.IsFalse(new XmlComparer().AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenXmlIsMissingGroupComparerFails() {
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild></grandchild></child><child></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child><grandchild></grandchild></child></root>");

			Assert.IsFalse(new XmlComparer().AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenXmlHasGroupsAndAreTheSameComparerSucceeds() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child><child></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child><grandchild>hello</grandchild></child><child></child></root>");

			Assert.IsTrue(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenAttributesAreTheSameComparerSucceeds() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child test='1'><grandchild>hello</grandchild></child></root>");

			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child test='1'><grandchild>hello</grandchild></child></root>");

			Assert.IsTrue(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenValuesAreDifferentComparerFails() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>test</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");

			Assert.IsFalse(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenXmlIsNotEqualComparerFails() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child>hello</child></root>");
			Assert.IsFalse(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenXmlStructureIsEqualButValuesDifferentComparerFails() {
			XmlComparer comparer = new XmlComparer();
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child>hello<grandchild></grandchild></child></root>");

			Assert.IsFalse(comparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenComparerIsPassedStringsItWorks() {
			string xmlA = "<root><child><grandchild>hello</grandchild></child></root>";
			string xmlB = "<root><child><grandchild>hello</grandchild></child></root>";

			Assert.IsTrue(new XmlComparer().AreEqual(xmlA, xmlB));
		}

		[TestMethod]
		public void WhenXmlContainsNestedGroupsValidationPasses() {
			string xml = @"
				<root>
					<children>
						<child name='John'></child>
						<child name='Joe'></child>
						<child name='Phil'>
							<grandchildren>
								<grandchild name='Jake'></grandchild>
								<grandchild name='Jack'></grandchild>
							</grandchildren>
						</child>
					</children>
				</root>";

			Assert.IsTrue(new XmlComparer().AreEqual(xml, xml));
		}
		private void AssertResults(string expectedResults, ComparisonResults actualResults) {
			StringBuilder sb = new StringBuilder();

			for (short i = 0; i < actualResults.Results.Count; i++) {
				ComparisonResult result = actualResults.Results[i];
				sb.AppendLine(result.Node + ":" + result.AreEqual);
			}

			string actualResultsStr = sb.ToString().Replace("\r\n", "");
			Assert.AreEqual(expectedResults, actualResultsStr);
		}

	}

}