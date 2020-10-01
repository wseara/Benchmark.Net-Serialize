using System;

namespace Benchmark.Models
{
    public readonly struct SecurityReadOnlyStruct
    {
        public SecurityReadOnlyStruct(
                                        decimal investedCapital,
                                        decimal netValue,
                                        decimal grossValue,
                                        decimal quantity,
                                        DateTime maturity,
                                        decimal ir,
                                        decimal iof,
                                        decimal otherFee,
                                        decimal totalTaxes,
                                        string index,
                                        bool settlement,
                                        string nickname,
                                        decimal financialValueCurrent,
                                        int custodyId,
                                        decimal unitPrice,
                                        string symbolId
            )
        {
            InvestedCapital = investedCapital;
            NetValue = netValue;
            GrossValue = grossValue;
            Quantity = quantity;
            Maturity = maturity;
            Ir = ir;
            Iof = iof;
            OtherFee = otherFee;
            TotalTaxes = totalTaxes;
            Index = index;
            Settlement = settlement;
            Nickname = nickname;
            FinancialValueCurrent = financialValueCurrent;
            CustodyId = custodyId;
            UnitPrice = unitPrice;
            SymbolId = symbolId;
        }

        public decimal InvestedCapital { get; }

        public decimal NetValue { get; }

        public decimal GrossValue { get; }

        public decimal Quantity { get; }

        public DateTime Maturity { get; }

        public decimal Ir { get; }

        public decimal Iof { get; }

        public decimal OtherFee { get; }

        public decimal TotalTaxes { get; }

        public string Index { get; }

        public bool Settlement { get; }

        public bool DailySettlement => true;

        public string FixedIncomeSecurityType => "TD";

        public string Nickname { get; }

        public string SecurityTypeName => "Tesouro Direto";

        public decimal FinancialValueCurrent { get; }

        public int CustodyId { get; }

        public decimal UnitPrice { get; }

        public string SymbolId { get; }
    }
}
