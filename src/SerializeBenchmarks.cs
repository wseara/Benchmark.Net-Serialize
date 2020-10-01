using AutoFixture;
using Benchmark.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Exporters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    [KeepBenchmarkFiles]
    [Config(typeof(AllowNonOptimized))]
    [RPlotExporter, RankColumn]
    public class SerializeBenchmarks
    {
        private IEnumerable<CustodyTdResponseClass> listClass;
        private IEnumerable<CustodyTdResponseStruct> listStruct;
        private IEnumerable<CustodyTdResponseReadOnlyStruct> listReadOnlyStruct;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var _class = new Fixture();
            listClass = _class.CreateMany<CustodyTdResponseClass>(1000);
            listStruct = PopularStruct(listClass);
            listReadOnlyStruct = PopularReadOnlyStruct(listClass);
        }

        private IEnumerable<CustodyTdResponseStruct> PopularStruct(IEnumerable<CustodyTdResponseClass> listClass)
        {
            List<CustodyTdResponseStruct> list = new List<CustodyTdResponseStruct>();
            foreach (var item in listClass)
            {
                list.Add(new CustodyTdResponseStruct()
                {
                    Securities = PopularStruct(item.Securities),
                    Cache = item.Cache,
                    UpdateDate = item.UpdateDate
                });
            }
            return list;
        }

        private IEnumerable<SecurityStruct> PopularStruct(IEnumerable<SecurityClass> listClass)
        {
            List<SecurityStruct> list = new List<SecurityStruct>();
            foreach (var item in listClass)
            {
                list.Add(new SecurityStruct()
                {
                    CustodyId = item.CustodyId,
                    FinancialValueCurrent = item.FinancialValueCurrent,
                    GrossValue = item.GrossValue,
                    Index = item.Index,
                    InvestedCapital = item.InvestedCapital,
                    Iof = item.Iof,
                    Ir = item.Ir,
                    Maturity = item.Maturity,
                    NetValue = item.NetValue,
                    Nickname = item.Nickname,
                    OtherFee = item.OtherFee,
                    Quantity = item.Quantity,
                    Settlement = item.Settlement,
                    SymbolId = item.SymbolId,
                    TotalTaxes = item.TotalTaxes,
                    UnitPrice = item.UnitPrice

                });
            }
            return list;
        }

        private IEnumerable<CustodyTdResponseReadOnlyStruct> PopularReadOnlyStruct(IEnumerable<CustodyTdResponseClass> listClass)
        {
            List<CustodyTdResponseReadOnlyStruct> list = new List<CustodyTdResponseReadOnlyStruct>();
            foreach (var item in listClass)
            {
                list.Add(new CustodyTdResponseReadOnlyStruct(PopularReadOnlyStruct(item.Securities), item.UpdateDate, item.Cache));
            }
            return list;
        }

        private IEnumerable<SecurityReadOnlyStruct> PopularReadOnlyStruct(IEnumerable<SecurityClass> listClass)
        {
            List<SecurityReadOnlyStruct> list = new List<SecurityReadOnlyStruct>();
            foreach (var item in listClass)
            {
                list.Add(new SecurityReadOnlyStruct(
                    custodyId: item.CustodyId,
                    financialValueCurrent: item.FinancialValueCurrent,
                    grossValue: item.GrossValue,
                    index: item.Index,
                    investedCapital: item.InvestedCapital,
                    iof: item.Iof,
                    ir: item.Ir,
                    maturity: item.Maturity,
                    netValue: item.NetValue,
                    nickname: item.Nickname,
                    otherFee: item.OtherFee,
                    quantity: item.Quantity,
                    settlement: item.Settlement,
                    symbolId: item.SymbolId,
                    totalTaxes: item.TotalTaxes,
                    unitPrice: item.UnitPrice
                ));
            }
            return list;
        }

        [GlobalCleanup]
        public void IterationCleanup()
        {
           
        }

        #region Default
        [Benchmark]
        public string Serialize_Struct()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(listStruct);
            return json;
        }

        [Benchmark]
        public string Serialize_Class()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(listClass);
            return json;
        }

        [Benchmark]
        public string Newton_Serialize_Struct()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(listStruct);
            return json;
        }

        [Benchmark]
        public string Newton_Serialize_Class()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(listClass);
            return json;
        }

        [Benchmark]
        public string Serialize_Struct_ReadOnly()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(listReadOnlyStruct);
            return json;
        }

        [Benchmark]
        public string Newton_Serialize_Struct_ReadOnly()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(listReadOnlyStruct);
            return json;
        }

        [Benchmark]
        public async ValueTask<string> Serialize_Objetct_Class_To_String_Byte()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(ms, listClass);
                ms.Position = 0;
                using var reader = new StreamReader(ms);
                return await reader.ReadToEndAsync();
            }
        }

        [Benchmark]
        public async ValueTask<string> Serialize_Objetct_Struct_To_String_Byte()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(ms, listStruct);
                ms.Position = 0;
                using var reader = new StreamReader(ms);
                return await reader.ReadToEndAsync();
            }
        }

        [Benchmark]
        public async ValueTask<string> Serialize_Objetct_Struct_ReadOnly_To_String_Byte()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(ms, listReadOnlyStruct);
                ms.Position = 0;
                using var reader = new StreamReader(ms);
                return await reader.ReadToEndAsync();
            }
        }

        [Benchmark]
        public string Serialize_Objetct_Class_To_String_Span()
        {
            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(listClass);
            ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
            return Encoding.UTF8.GetString(readOnlySpan);
        }

        [Benchmark]
        public string Serialize_Objetct_Struct_To_String_Span()
        {
            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(listStruct);
            ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
            return Encoding.UTF8.GetString(readOnlySpan);
        }

        [Benchmark]
        public string Serialize_Objetct_Struct_ReadOnly_To_String_Span()
        {
            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(listReadOnlyStruct);
            ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(jsonUtf8Bytes);
            return Encoding.UTF8.GetString(readOnlySpan);
        }
        #endregion
    }

    
}