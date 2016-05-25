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
		public void Test() {
			string xml = @"<root><child><grandchild>hello</grandchild></child></root>";
			XmlValidator validator = new XmlValidator(xml);
			var node = validator.Xml
				.Where(x => x.Name.Equals("grandchild") && x.Value.Equals("hello"))
				.SingleOrDefault();

			Assert.IsNotNull(node);
		}

	}

}
