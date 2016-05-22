using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace XmlComparer {

	public class XmlComparer {
		private Dictionary<Guid, bool> _results;

		public XmlComparer() {
			_results = new Dictionary<Guid, bool>();
		}

		public bool AreEqual(XmlDocument xmlObjA, XmlDocument xmlObjB) {
			Compare(xmlObjA.FirstChild, xmlObjB.FirstChild);
			bool isDifferent = _results.Any(kvp => kvp.Value == false);

			if (isDifferent) {
				return false;
			}
			else {
				return true;
			}
		}

		private void Compare(XmlNode nodeA, XmlNode nodeB) {
			if ((nodeA == null && nodeB != null) ||
				(nodeA != null && nodeB == null)) {

				_results.Add(Guid.NewGuid(), false);
				return;
			}

			if (nodeA.HasChildNodes && nodeB.HasChildNodes) {
				Compare(nodeA.FirstChild, nodeB.FirstChild);
			}

			if (nodeA.NextSibling != null && nodeB.NextSibling != null) {
				Compare(nodeA.NextSibling, nodeB.NextSibling);
			}

			if ((nodeA.NextSibling == null && nodeB.NextSibling != null) ||
				(nodeA.NextSibling != null && nodeB.NextSibling == null)) {

				_results.Add(Guid.NewGuid(), false);
				return;
			}

			if (!nodeA.Name.Equals(nodeB.Name)) {
				_results.Add(Guid.NewGuid(), false);
				return;
			}

			bool nodeAHasAttributes = HasAttributes(nodeA);
			bool nodeBHasAttributes = HasAttributes(nodeB);

			if (nodeAHasAttributes && nodeBHasAttributes) {
				bool attributesAreTheSame = AreAttributesTheSame(nodeA.Attributes, nodeB.Attributes);

				if (!attributesAreTheSame) {
					_results.Add(Guid.NewGuid(), false);
					return;
				}
			}

			if ((nodeAHasAttributes && !nodeBHasAttributes) ||
				(!nodeAHasAttributes && nodeBHasAttributes)) {

				_results.Add(Guid.NewGuid(), false);
				return;
			}

			if (nodeA.Value != null || nodeB.Value != null) {
				bool areTheSame = AreValuesTheSame(nodeA.Value, nodeB.Value);

				if (!areTheSame) {
					_results.Add(Guid.NewGuid(), false);
					return;
				}
			}

			_results.Add(Guid.NewGuid(), true);
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

		private bool AreAttributesTheSame(XmlAttributeCollection collectionA, XmlAttributeCollection collectionB) {
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

	}

}