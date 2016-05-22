using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlComparer.Tests {

	[TestClass]
	public class XmlComparerTests {

		[TestMethod]
		public void WhenXmlIsEqualComparerSucceeds() {
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");

			Assert.IsTrue(XmlComparer.AreEqual(xmlDocs[0], xmlDocs[0]));
		}

		[TestMethod]
		public void WhenAttributesAreDifferentComparerFails() {
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child test='1'><grandchild>hello</grandchild></child></root>");

			Assert.IsFalse(XmlComparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenXmlIsNotEqualComparerFails() {
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child>hello</child></root>");
			Assert.IsFalse(XmlComparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		[TestMethod]
		public void WhenXmlStructureIsEqualButValuesDifferentComparerFails() {
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[0].LoadXml(@"<root><child><grandchild>hello</grandchild></child></root>");
			xmlDocs[1] = new XmlDocument();
			xmlDocs[1].LoadXml(@"<root><child>hello<grandchild></grandchild></child></root>");

			Assert.IsFalse(XmlComparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}
	}

}
