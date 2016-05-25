using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace XmlComparer {

	public class XmlValidator {
		private XmlDocument _xmlDoc;
		private List<XmlNode> _flattenedXml;

		private XmlValidator() {
			_xmlDoc = new XmlDocument();
			_flattenedXml = new List<XmlNode>();
		}

		public XmlValidator(string xml) :
			base() {

			_xmlDoc.LoadXml(xml);
			this.Flatten(_xmlDoc);
		}

		public XmlValidator(XmlDocument xml) :
			base() {

			_xmlDoc = xml;
			this.Flatten(_xmlDoc.FirstChild);
		}

		public XmlValidator(XDocument xml) :
			base() {

			using (var xmlReader = xml.CreateReader()) {
				_xmlDoc.Load(xmlReader);
			}
			this.Flatten(_xmlDoc.FirstChild);
		}

		public List<XmlNode> Xml {
			get { return _flattenedXml; }
		}

		private void Flatten(XmlNode node) {
			if (node == null) {
				return;
			}

			if (node.NextSibling != null) {
				Flatten(node.NextSibling);
			}
			
			_flattenedXml.Add(node);
		}
	}

}
