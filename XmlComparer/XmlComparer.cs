using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace XmlComparer {

	public static class XmlComparer {
		private static List<string> _xmlObjStructure = new List<string>();
		private static int _elementsPerDocument = 0;

		public static bool AreEqual(XmlDocument xmlObjA, XmlDocument xmlObjB) {
			BuildXmlStructure(xmlObjA.FirstChild);
			BuildXmlStructure(xmlObjB.FirstChild);

			if ((_xmlObjStructure.Count % 2) != 0) {
				return false;
			}

			_elementsPerDocument = (int)_xmlObjStructure.Count / 2;

			return false;
		}

		private static void BuildXmlStructure(XmlNode node) {
			if (node == null) {
				return;
			}

			if (node.HasChildNodes) {
				BuildXmlStructure(node.FirstChild);
				_xmlObjStructure.Add(node.Name);
			}
		}

		private static bool AreValuesEqual(XmlElement elementA, XmlElement elementB) {
			return elementA.Value.Equals(elementB.Value);
		}
	}

}