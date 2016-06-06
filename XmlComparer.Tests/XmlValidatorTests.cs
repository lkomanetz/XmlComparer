using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlComparer.Tests {

	[TestClass]
	public class XmlValidatorTests {
		
		[TestMethod]
		public void WhenNoGroupsExistValidatorReturnsZero() {
			string xml = @"<root><child><grandchild>hello</grandchild></child></root>";
			XmlValidator validator = new XmlValidator(xml);

			Assert.IsTrue(validator.GroupNodes.Count == 0);
		}

		[TestMethod]
		public void WhenOneGroupExistsValidatorReturnsOne() {
			string xml = @"
				<root>
					<child>
						<grandchild></grandchild>
						<grandchild></grandchild>
					</child>
				</root>";

			XmlValidator validator = new XmlValidator(xml);

			Assert.IsTrue(validator.GroupNodes.Count == 1);
			Assert.IsTrue(validator.GroupNodes[0] == "grandchild");
		}

		[TestMethod]
		public void WhenMultipleGroupsExistValidatrReturnsCorrectAmount() {
			string xml = @"
				<root>
					<child>
						<grandchild></grandchild>
						<grandchild></grandchild>
					</child>
					<child>
						<grandchild></grandchild>
						<grandchild></grandchild>
					</child>
				</root>";

			XmlValidator validator = new XmlValidator(xml);
			Assert.IsTrue(validator.GroupNodes.Count == 2, "Did not find all group nodes");
			Assert.IsTrue(
				validator.GroupNodes.Contains("child") && validator.GroupNodes.Contains("grandchild"),
				"Incorrect group nodes found."
			);
		}

		[TestMethod]
		public void WhenSameNodeIsFoundOutsideOfGroupNoGroupNodesFound() {
			string xml = @"
				<root>
					<child>
						<grandchild></grandchild>
					</child>
					<child>
						<grandchild></grandchild>
					</child>
				</root>";

			XmlValidator validator = new XmlValidator(xml);
			Assert.IsTrue(validator.GroupNodes.Count == 1, "Did not find child to be group node.");
			Assert.IsTrue(
				validator.GroupNodes.Contains("child"),
				"Validator did not find correct group nodes."
			);
		}
	}

}
