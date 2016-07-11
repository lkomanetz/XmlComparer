using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlComparer.Contracts {
	public class ComparisonResult {

		public bool AreEqual { get; internal set; }
		public string Node { get; internal set; }

	}
}
