using BenchmarkDotNet.Running;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        static async Task Main(string[] args)     
        => BenchmarkRunner.Run<DeserializeBenchmarks>();
    }
}
