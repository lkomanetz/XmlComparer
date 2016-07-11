using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlComparer.Contracts {

	public class ComparisonResults {

		public ComparisonResults() {
			this.Results = new List<ComparisonResult>();
		}

		public bool IsSuccessful { get; set; }
		public IList<ComparisonResult> Results { get; private set; }

		public void Add(ComparisonResult result) {
			this.Results.Add(result);
		}
	}
	
}
