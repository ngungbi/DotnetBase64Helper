// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Benchmarks;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<Sha256Benchmark>();
// BenchmarkRunner.Run<StringToBase64Benchmark>();
