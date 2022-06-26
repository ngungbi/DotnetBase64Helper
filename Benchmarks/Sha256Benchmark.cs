using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Ngb.Base64Helper.Sha256;

namespace Benchmarks;

[MemoryDiagnoser]
public class Sha256Benchmark {
    private const string Input = "Hello world!";

    [Benchmark]
    public string TraditionalSha256Create() {
        using var sha256 = SHA256.Create();
        var inputBytes = Encoding.UTF8.GetBytes(Input);
        var result = sha256.ComputeHash(inputBytes);
        return Convert.ToBase64String(result);
    }

    [Benchmark]
    public string TraditionalSha56Shared() {
        var inputBytes = Encoding.UTF8.GetBytes(Input);
        var result = SHA256.HashData(inputBytes);
        return Convert.ToBase64String(result);
    }

    [Benchmark]
    public string CustomSha256() {
        return Input.ToSha256Base64String();
    }
    
    [Benchmark]
    public string CustomSha256WithSpan() {
        return Input.AsSpan().ToSha256Base64String();
    }
}
