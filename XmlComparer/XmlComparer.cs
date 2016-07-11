using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using XmlComparer.Contracts;

namespace XmlComparer {

	public class XmlComparer {
		private ComparisonResults _results;
		private XmlDocument _xmlDocA;
		private XmlDocument _xmlDocB;

		public XmlComparer() {
			_results = new ComparisonResults();
			_xmlDocA = new XmlDocument();
			_xmlDocB = new XmlDocument();
		}

		public ComparisonResults Results {
			get { return _results; }
		}

		public bool AreEqual(XmlDocument xmlObjA, XmlDocument xmlObjB) {
			_xmlDocA = xmlObjA;
			_xmlDocB = xmlObjB;

			bool areEqual = CompareEquality();
			return areEqual;
		}

		public bool AreEqual(string xmlStringA, string xmlStringB) {
			_xmlDocA.LoadXml(xmlStringA);
			_xmlDocB.LoadXml(xmlStringB);

			bool areEqual = CompareEquality();
			return areEqual;
		}

		public bool AreEqual(XDocument xDocA, XDocument xDocB) {
			_xmlDocA = ToXmlDocument(xDocA);
			_xmlDocB = ToXmlDocument(xDocB);

			bool areEqual = CompareEquality();
			return areEqual;
		}

		private bool CompareEquality() {
			Compare(_xmlDocA.FirstChild, _xmlDocB.FirstChild);

			bool isDifferent = _results.Results.Any(x => !x.AreEqual);
			if (isDifferent) {
				return false;
			}
			else {
				return true;
			}
		}

		private void Compare(XmlNode nodeA, XmlNode nodeB) {
			ComparisonResult result = new ComparisonResult() {
				Node = nodeA.Name
			};

			if ((nodeA == null && nodeB != null) ||
				(nodeA != null && nodeB == null)) {

				result.AreEqual = false;
				_results.Add(result);
				return;
			}

			if (nodeA.HasChildNodes && nodeB.HasChildNodes) {
				Compare(nodeA.FirstChild, nodeB.FirstChild);
			}

			/*
			 * We can only recursively call on each sibling after we are
			 * done checking for children.
			 */
			if (nodeA.NextSibling != null && nodeB.NextSibling != null) {
				Compare(nodeA.NextSibling, nodeB.NextSibling);
			}

			if ((nodeA.NextSibling == null && nodeB.NextSibling != null) ||
				(nodeA.NextSibling != null && nodeB.NextSibling == null)) {

				result.AreEqual = false;
				_results.Add(result);
				return;
			}

			if (!nodeA.Name.Equals(nodeB.Name)) {
				result.AreEqual = false;
				_results.Add(result);
				return;
			}

			if (!CompareAttributes(nodeA, nodeB, ref result)) {
				_results.Add(result);
				return;
			}

			if (nodeA.Value != null || nodeB.Value != null) {
				bool areTheSame = AreValuesTheSame(nodeA.Value, nodeB.Value);

				if (!areTheSame) {
					result.AreEqual = false;
					_results.Add(result);
					return;
				}
			}

			result.AreEqual = true;
			_results.Add(result);
		}

		private bool CompareAttributes(XmlNode nodeA, XmlNode nodeB, ref ComparisonResult result) {
			bool nodeAHasAttributes = HasAttributes(nodeA);
			bool nodeBHasAttributes = HasAttributes(nodeB);

			if (nodeAHasAttributes && nodeBHasAttributes) {
				bool attributesAreTheSame = AreAttributesTheSame(nodeA.Attributes, nodeB.Attributes);

				if (!attributesAreTheSame) {
					result.AreEqual = false;
					return false;
				}
			}

			if ((nodeAHasAttributes && !nodeBHasAttributes) ||
				(!nodeAHasAttributes && nodeBHasAttributes)) {

				result.AreEqual = false;
				return false;
			}

			return true;
		}

		private bool HasAttributes(XmlNode node) {
			return node.Attributes != null && node.Attributes.Count > 0;
		}

		private bool AreValuesTheSame(string objA, string objB) {
			if ((objA == null && objB != null) ||
				(objA != null && objB == null)) {

				return false;
			}

			return objA == objB;
		}

		private bool AreAttributesTheSame(
			XmlAttributeCollection collectionA,
			XmlAttributeCollection collectionB
		) {
			if ((collectionA == null && collectionB != null) ||
				(collectionA != null && collectionB == null)) {

				return false;
			}

			if (collectionA.Count != collectionB.Count) {
				return false;
			}

			for (int i = 0; i < collectionA.Count; i++) {
				if (!collectionA[i].Name.Equals(collectionB[i].Name)) {
					return false;
				}

				if (!collectionA[i].Value.Equals(collectionB[i].Value)) {
					return false;
				}
			}

			return true;
		}

		private XmlDocument ToXmlDocument(XDocument xDoc) {
			var xmlDoc = new XmlDocument();

			using (var xmlReader = xDoc.CreateReader()) {
				xmlDoc.Load(xmlReader);
			}

			return xmlDoc;
		}
	}

}