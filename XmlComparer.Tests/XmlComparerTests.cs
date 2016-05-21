using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlComparer.Tests {

	[TestClass]
	public class XmlComparerTests {

		[TestMethod]
		public void WhenXmlIsEqualComparerSucceeds() {
			XmlDocument[] xmlDocs = CreateDocuments();
			Assert.IsTrue(XmlComparer.AreEqual(xmlDocs[0], xmlDocs[0]));
		}

		[TestMethod]
		public void WhenXmlIsNotEqualComparerFails() {
			XmlDocument[] xmlDocs = CreateDocuments();
			Assert.IsFalse(XmlComparer.AreEqual(xmlDocs[0], xmlDocs[1]));
		}

		private XmlDocument[] CreateDocuments() {
			XmlDocument[] xmlDocs = new XmlDocument[2];
			xmlDocs[0] = new XmlDocument();
			xmlDocs[1] = new XmlDocument();
			string xmlStringA = @"<root><child><grandchild>hello</grandchild></child></root>";
			string xmlStringB = @"<root><child>hello</child></root>";

			xmlDocs[0].LoadXml(xmlStringA);
			xmlDocs[1].LoadXml(xmlStringB);

			return xmlDocs;
		}

	}

}
