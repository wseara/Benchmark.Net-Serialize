using System;

namespace Benchmark.Models
{
    public class SecurityClass
	{
		public decimal InvestedCapital { get; set; }

		public decimal NetValue { get; set; }

		public decimal GrossValue { get; set; }

		public decimal Quantity { get; set; }

		public DateTime Maturity { get; set; }

		public decimal Ir { get; set; }

		public decimal Iof { get; set; }

		public decimal OtherFee { get; set; }

		public decimal TotalTaxes { get; set; }

		public string Index { get; set; }

		public bool Settlement { get; set; }

		public bool DailySettlement => true;

		public string FixedIncomeSecurityType => "TD";

		public string Nickname { get; set; }

		public string SecurityTypeName => "Tesouro Direto";

		public decimal FinancialValueCurrent { get; set; }

		public int CustodyId { get; set; }

		public decimal UnitPrice { get; set; }

		public string SymbolId { get; set; }
	}
}
    