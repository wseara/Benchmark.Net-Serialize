using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    public class CustodyTdResponseClass
	{
		public IEnumerable<SecurityClass> Securities { get; set; }
		public DateTime UpdateDate { get; set; }
		public bool Cache { get; set; }
	}
}
