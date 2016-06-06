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
		private Dictionary<string, int> _nodeCounts;

		private XmlValidator() {
			_xmlDoc = new XmlDocument();
			_nodeCounts = new Dictionary<string,int>();
			this.GroupNodes = new List<string>();
		}

		public XmlValidator(string xml) 
			: this() {
			this.GroupNodes = new List<string>();
			_xmlDoc.LoadXml(xml);

			this.FindGroupNodes(_xmlDoc.FirstChild);
		}

		public XmlValidator(XmlDocument xml) 
			: this() {
			_xmlDoc = xml;

			this.FindGroupNodes(_xmlDoc.FirstChild);
		}

		public XmlValidator(XDocument xml) 
			: this() {

			using (var xmlReader = xml.CreateReader()) {
				_xmlDoc.Load(xmlReader);
			}

			this.FindGroupNodes(_xmlDoc.FirstChild);
		}

		public List<string> GroupNodes { get; set; }

		private void FindGroupNodes(XmlNode node) {
			if (node == null) {
				return;
			}

			if (node.HasChildNodes) {
				FindGroupNodes(node.FirstChild);
			}

			if (node.NextSibling != null) {
				//FindGroupNodes(node.NextSibling);
				int count = 1;
				_nodeCounts.TryGetValue(node.Name, out count);

				if (this.GroupNodes.Contains(node.Name)) {
					return;
				}
				else {
					this.GroupNodes.Add(node.Name);
				}
			}
		}

	}

}
