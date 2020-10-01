using AutoFixture;
using Benchmark.Models;
using Benchmark.Utils;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Benchmark
{
    [KeepBenchmarkFiles]
    [Config(typeof(AllowNonOptimized))]
    [RPlotExporter, RankColumn]
    public class DeserializeBenchmarks
    {
        private string listClass;
        private JsonSerializerOptions serializerOptions;
        private Consumer consumer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var _class = new Fixture();
            var _objs = _class.CreateMany<CustodyTdResponseClass>(1000);

            listClass = System.Text.Json.JsonSerializer.Serialize(_objs);

            //Utilizo isso por que as structs readonly não es´tão deserializando de forma facil com o .net core 3.1
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new ImmutableConverter());
            serializerOptions.Converters.Add(new ListConverter<SecurityReadOnlyStruct>());
            serializerOptions.Converters.Add(new ListConverter<CustodyTdResponseReadOnlyStruct>());

            consumer = new Consumer();
        }

        [GlobalCleanup]
        public void IterationCleanup()
        {

        }

        #region Populate
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
        #endregion

        #region Default
        [Benchmark]
        public void Deserialize_Struct()
        {
            var json = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CustodyTdResponseStruct>>(listClass);
            consumer.Consume(json);
        }

        [Benchmark]
        public void Deserialize_Class()
        {
            var json = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CustodyTdResponseClass>>(listClass);
            consumer.Consume(json);
        }

        [Benchmark]
        public void Newton_Deserialize_Struct()
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CustodyTdResponseStruct>>(listClass);
            consumer.Consume(json);
        }

        [Benchmark]
        public void Newton_Deserialize_Class()
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CustodyTdResponseClass>>(listClass);
            consumer.Consume(json);
        }

        [Benchmark]
        public void Deserialize_Struct_ReadOnly()
        {
            var json = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CustodyTdResponseReadOnlyStruct>>(listClass, serializerOptions);
            consumer.Consume(json);
        }

        [Benchmark]
        public void Newton_Deserialize_Struct_ReadOnly()
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CustodyTdResponseReadOnlyStruct>>(listClass);
            consumer.Consume(json);
        }

        [Benchmark]
        public async ValueTask Deserialize_String_Byte_TO_Objetct_Class()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(listClass);

            using MemoryStream ms = new MemoryStream(byteArray);
            var json = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<CustodyTdResponseClass>>(ms);
            consumer.Consume(json);
        }

        [Benchmark]
        public async ValueTask Deserialize_String_Byte_To_Objetct_Struct()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(listClass);

            using MemoryStream ms = new MemoryStream(byteArray);
            var json = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<CustodyTdResponseStruct>>(ms);
            consumer.Consume(json);
        }

        [Benchmark]
        public async ValueTask Deserialize_String_Byte_To_Objetct_Struct_ReadOnly()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(listClass);

            using MemoryStream ms = new MemoryStream(byteArray);
            var json = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<CustodyTdResponseReadOnlyStruct>>(ms, serializerOptions);

            consumer.Consume(json);
        }

        /// <summary>
        /// Se mostrou mais performatico para converter uma string json para um objeto
        /// </summary>
        [Benchmark]
        public void Deserialize_String_Span_To_Objetct_Class()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(listClass);
            ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(byteArray);

            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CustodyTdResponseClass>>(readOnlySpan);
            consumer.Consume(jsonUtf8Bytes);
        }

        /// <summary>
        /// Se mostrou mais performatico para converter uma string json para um objeto
        /// </summary>
        [Benchmark]
        public void Deserialize_String_Span_Objetct_Struct()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(listClass);
            ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(byteArray);

            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CustodyTdResponseStruct>>(readOnlySpan);
            consumer.Consume(jsonUtf8Bytes);
        }

        /// <summary>
        /// Consumiu muita memoria devido aos converters
        /// </summary>
        [Benchmark]
        public void Deserialize_String_Span_To_Objetct_Struct_ReadOnly()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(listClass);
            ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(byteArray);

            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CustodyTdResponseReadOnlyStruct>>(readOnlySpan, serializerOptions);
            consumer.Consume(jsonUtf8Bytes);
        }
        #endregion
    }
}