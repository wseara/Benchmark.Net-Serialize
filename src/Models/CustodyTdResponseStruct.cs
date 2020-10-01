using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    public struct CustodyTdResponseStruct
	{
		public IEnumerable<SecurityStruct> Securities { get; set; }
		public DateTime UpdateDate { get; set; }
		public bool Cache { get; set; }
	}
}
