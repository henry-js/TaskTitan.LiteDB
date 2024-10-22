using BenchmarkDotNet.Running;
using TaskTitan.Benchmarks;

var summary = BenchmarkRunner.Run<LiteDbBenchmark>();
