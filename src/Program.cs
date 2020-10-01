using BenchmarkDotNet.Running;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        //Mudar aqui quando quiser rodar somente um dos testes
        static async Task Main(string[] args)     
        => BenchmarkRunner.Run<SerializeBenchmarks>();
    }
}
