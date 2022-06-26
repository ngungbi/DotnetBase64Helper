using System;
using System.Security.Cryptography;
using System.Text;
using Ngb.Base64Helper;
using Ngb.Base64Helper.Sha512;
using NUnit.Framework;

namespace Tests; 

public class Sha512Test {
    [SetUp]
    public void Setup(){}

    [Test]
    [TestCase("halo dunia", "1QoOR6gTB7NolJJtuTpcqmh7k4EmbbANQ8KaIrmuazga45vPhIRzqAfvUvuH5h/dTgUbkNzSbJ+NpzR2gLN1IA==")]
    [TestCase("coba lagi", "5udofp3uGvphAGJpi46nz4/ebeEYLzM4X9Pw6uTBgwPTLJsbmG/Pvpr94aiKRrJFUrwZpATXyNbwJoNxXennNw==")]
    public void ToSha512FromStringTest(string input, string expected) {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var resultBytes = SHA512.HashData(inputBytes);
        var reference = resultBytes.ToBase64String();
        Assert.AreEqual(expected, reference);
        // Console.WriteLine(result);
        var result = input.ToSha512Base64String();
        Assert.AreEqual(result, expected);
    }
    
    [Test]
    [TestCase("halo dunia", "1QoOR6gTB7NolJJtuTpcqmh7k4EmbbANQ8KaIrmuazga45vPhIRzqAfvUvuH5h_dTgUbkNzSbJ.NpzR2gLN1IA")]
    [TestCase("coba lagi", "5udofp3uGvphAGJpi46nz4_ebeEYLzM4X9Pw6uTBgwPTLJsbmG_Pvpr94aiKRrJFUrwZpATXyNbwJoNxXennNw")]
    public void ToSha512FromStringUriSafeTest(string input, string expected) {
        var result = input.ToSha512Base64String(true);
        Assert.AreEqual(result, expected);
    }
}
