using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace XmlComparer {

	public static class XmlComparer {
		private static Dictionary<string, string> _attributeStructure = new Dictionary<string,string>();
		private static List<string> _valueStructure = new List<string>();
		private static List<string> _xmlObjStructure = new List<string>();
		private static int _elementsPerDocument = 0;

		public static bool AreEqual(XmlDocument xmlObjA, XmlDocument xmlObjB) {
			BuildXmlStructure(xmlObjA.FirstChild);
			BuildXmlStructure(xmlObjB.FirstChild);

			if ((_xmlObjStructure.Count % 2) != 0) {
				return false;
			}

			_elementsPerDocument = (int)_xmlObjStructure.Count / 2;

			if (AreStructuresEqual()) {
				BuildValueStructure(xmlObjA.FirstChild);
				BuildValueStructure(xmlObjB.FirstChild);

				BuildAttributeStructure(xmlObjA.FirstChild);
				BuildAttributeStructure(xmlObjB.FirstChild);
			}


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

		private static void BuildAttributeStructure(XmlNode node) {
			if (node == null) {
				return;
			}

			if (node.HasChildNodes) {
				BuildAttributeStructure(node.FirstChild);
			}

			if (node.Attributes != null &&
				node.Attributes.Count > 0) {

				for (int i = 0; i < node.Attributes.Count; i++) {
					_attributeStructure.Add(node.Attributes[i].Name, node.Attributes[i].Value);
				}
			}
		}

		private static void BuildValueStructure(XmlNode node) {
			if (node == null) {
				 return;
			}

			if (node.HasChildNodes) {
				BuildValueStructure(node.FirstChild);
				_valueStructure.Add(node.InnerText);
			}
		}

		private static void SeparateStructures() {
		}

		private static bool AreStructuresEqual() {
			bool structuresAreEqual = true;
			List<string> firstStructure = new List<string>();
			List<string> secondStructure = new List<string>();

			for (int i = 0; i < _xmlObjStructure.Count; i++) {
				if (i <= _elementsPerDocument - 1) {
					firstStructure.Add(_xmlObjStructure[i]);
				}
				else {
					secondStructure.Add(_xmlObjStructure[i]);
				}
			}

			if (firstStructure.Count != secondStructure.Count) {
				return false;
			}

			for (int i = 0; i < _elementsPerDocument; i++) {
				structuresAreEqual = firstStructure[i].Equals(secondStructure[i]);

				if (!structuresAreEqual) {
					break;
				}
			}

			return structuresAreEqual;
		}

		private static bool AreValuesEqual(XmlElement elementA, XmlElement elementB) {
			return elementA.Value.Equals(elementB.Value);
		}
	}

}