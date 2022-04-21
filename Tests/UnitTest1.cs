using System;
using System.Security.Cryptography;
using System.Text;
using Base64;
using Base64.Sha256;
using NUnit.Framework;

namespace Tests;

public class Tests {
    [SetUp]
    public void Setup() { }

    [Test]
    [TestCase("halo dunia", "aGFsbyBkdW5pYQ==")]
    [TestCase("coba lagi", "Y29iYSBsYWdp")]
    [TestCase("mencoba hahaha", "bWVuY29iYSBoYWhhaGE=")]
    public void TestUsingBytes(string input, string output) {
        var bytes = Encoding.UTF8.GetBytes(input);
        var result = bytes.ToBase64String();
        Assert.AreEqual(result, output);
    }

    [Test]
    [TestCase("halo dunia", "aGFsbyBkdW5pYQ")]
    [TestCase("coba lagi", "Y29iYSBsYWdp")]
    [TestCase("mencoba hahaha", "bWVuY29iYSBoYWhhaGE")]
    public void TestUsingBytesWithoutPadding(string input, string output) {
        var bytes = Encoding.UTF8.GetBytes(input);
        var result = bytes.ToBase64String(true);
        Assert.AreEqual(result, output);
    }

    [Test]
    [TestCase("halo dunia", "aGFsbyBkdW5pYQ==")]
    [TestCase("coba lagi", "Y29iYSBsYWdp")]
    [TestCase("mencoba hahaha", "bWVuY29iYSBoYWhhaGE=")]
    public void TestUsingString(string input, string output) {
        // var bytes = Encoding.UTF8.GetBytes(input);
        var result = input.ToBase64String();
        Assert.AreEqual(result, output);

        var origin = output.ToStringFromBase64();
        Assert.AreEqual(origin, input);
    }

    [Test]
    [TestCase("halo dunia", "aGFsbyBkdW5pYQ")]
    [TestCase("coba lagi", "Y29iYSBsYWdp")]
    [TestCase("mencoba hahaha", "bWVuY29iYSBoYWhhaGE")]
    public void TestUsingStringWithoutPadding(string input, string output) {
        // var bytes = Encoding.UTF8.GetBytes(input);
        var result = input.ToBase64String(true);
        Assert.AreEqual(result, output);

        var origin = output.ToStringFromBase64();
        if (output.Length % 4 != 0) Assert.IsNull(origin);

        origin = output.ToStringFromBase64(true);
        Assert.AreEqual(origin, input);
    }

    [Test]
    [TestCase("halo dunia", "aGFsbyBkdW5pYQ==")]
    [TestCase("coba lagi", "Y29iYSBsYWdp")]
    [TestCase("mencoba hahaha", "bWVuY29iYSBoYWhhaGE=")]
    public void FromBase64ToBytes(string input, string output) {
        // var bytes = Encoding.UTF8.GetBytes(input);
        var reference = Encoding.UTF8.GetBytes(input);

        var origin = output.ToByteArrayFromBase64();
        Assert.AreEqual(origin, reference);

        origin = output.ToByteArrayFromBase64(true);
        Assert.AreEqual(origin, reference);
    }

    [Test]
    [TestCase("halo dunia", "aGFsbyBkdW5pYQ")]
    [TestCase("coba lagi", "Y29iYSBsYWdp")]
    [TestCase("mencoba hahaha", "bWVuY29iYSBoYWhhaGE")]
    public void FromBase64ToBytesWithoutPadding(string input, string output) {
        // var bytes = Encoding.UTF8.GetBytes(input);
        var reference = Encoding.UTF8.GetBytes(input);

        var origin = output.ToByteArrayFromBase64();
        if (output.Length % 4 != 0) Assert.IsNull(origin);

        origin = output.ToByteArrayFromBase64(true);
        Assert.AreEqual(origin, reference);
    }

    [Test]
    [TestCase("halo dunia","CbULL2heeng+7uAtgHDmcNSK8RiksA97/r6fjw5dPVI=")]
    [TestCase("coba lagi","uC0s/5+tL6tiMglHMkoeZodPnJUq9J/O7iajOvAo9lY=")]
    public void Sha256Test(string input, string output) {
        // var source = "halo dunia";
        string reference;
        using (var sha256 = SHA256.Create()) {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashed = sha256.ComputeHash(bytes);
            reference = Convert.ToBase64String(hashed);
            Assert.AreEqual(reference, output);
        }
        var result = input.ToSha256Base64String();
        Assert.AreEqual(result, reference);
    }
}
