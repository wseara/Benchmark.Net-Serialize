using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    public readonly struct CustodyTdResponseReadOnlyStruct
	{
        public CustodyTdResponseReadOnlyStruct(IEnumerable<SecurityReadOnlyStruct> securities, DateTime updateDate, bool cache)
		{
			Securities = securities;
			UpdateDate = updateDate;
			Cache = cache;
        }	

		public IEnumerable<SecurityReadOnlyStruct> Securities { get;  }
		public DateTime UpdateDate { get; }
		public bool Cache { get;  }
	}
}
