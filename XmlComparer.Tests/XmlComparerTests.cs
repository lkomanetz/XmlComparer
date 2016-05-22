using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

			Assert.IsTrue(comparer.AreEqual(xmlDocs[0], xmlDocs[0]));
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

	}

}