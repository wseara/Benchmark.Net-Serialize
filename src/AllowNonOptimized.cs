using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Validators;
using System.Linq;

namespace Benchmark
{
    public class AllowNonOptimized : ManualConfig
    {
        public AllowNonOptimized()
        {
            AddColumn(StatisticColumn.Min);
            AddColumn(StatisticColumn.Max);
            AddColumn(StatisticColumn.Iterations);

            foreach (var unrollFactor in new[] { 16 })
                foreach (var invocationCount in new[] { 16 })
                    AddJob(Job.InProcess
                        .WithLaunchCount(1)
                        .WithInvocationCount(invocationCount)
                        .WithUnrollFactor(unrollFactor)
                    );

            AddValidator(JitOptimizationsValidator.DontFailOnError);

            AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
            AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
            WithOptions(ConfigOptions.DisableOptimizationsValidator);
            AddDiagnoser(MemoryDiagnoser.Default);
            AddExporter(HtmlExporter.Default);
            AddExporter(BenchmarkReportExporter.Default);
        }
    }
}
