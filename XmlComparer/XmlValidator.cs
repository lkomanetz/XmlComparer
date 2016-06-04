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
		}

		public XmlValidator(string xml) {
			_xmlDoc = new XmlDocument();
			_nodeCounts = new Dictionary<string,int>();
			_xmlDoc.LoadXml(xml);

			this.FindGroupNodes(_xmlDoc.FirstChild);
			this.PopulateGroupNodesArray();

		}

		public XmlValidator(XmlDocument xml) {
			_nodeCounts = new Dictionary<string,int>();
			_xmlDoc = xml;

			this.FindGroupNodes(_xmlDoc.FirstChild);
			this.PopulateGroupNodesArray();
		}

		public XmlValidator(XDocument xml) {
			_nodeCounts = new Dictionary<string,int>();
			_xmlDoc = new XmlDocument();

			using (var xmlReader = xml.CreateReader()) {
				_xmlDoc.Load(xmlReader);
			}

			this.FindGroupNodes(_xmlDoc.FirstChild);
			this.PopulateGroupNodesArray();
		}

		public string[] GroupNodes { get; set; }

		private void FindGroupNodes(XmlNode node) {
			if (node == null) {
				return;
			}

			if (node.HasChildNodes) {
				FindGroupNodes(node.FirstChild);
			}

			if (node.NextSibling != null) {
				FindGroupNodes(node.NextSibling);
			}
			
			int count = 0;
			_nodeCounts.TryGetValue(node.Name, out count);

			if (_nodeCounts.ContainsKey(node.Name)) {
				_nodeCounts[node.Name]++;
			}
			else {
				_nodeCounts.Add(node.Name, ++count);
			}

		}

		private void PopulateGroupNodesArray() {
			var groupNodes = _nodeCounts
				.Where(x => x.Value > 1)
				.Select(x => x.Key)
				.ToList();

			this.GroupNodes = new string[groupNodes.Count];

			for (short i = 0; i < groupNodes.Count; i++) {
				this.GroupNodes[i] = groupNodes[i];
			}
		}

	}

}
