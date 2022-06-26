using System.Text;
using BenchmarkDotNet.Attributes;
using Ngb.Base64Helper;

namespace Benchmarks;

[MemoryDiagnoser]
public class StringToBase64Benchmark {
    [Benchmark]
    public string TraditionalBase64() {
        var input = "Lorem ipsum dolor sit amet!";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }

    [Benchmark]
    public string WithBase64Helper() {
        var input = "Lorem ipsum dolor sit amet!";
        return input.ToBase64String();
    }
}
