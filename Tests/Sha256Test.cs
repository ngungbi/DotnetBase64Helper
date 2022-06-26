using System;
using System.Security.Cryptography;
using System.Text;
using Ngb.Base64Helper.Sha256;
using NUnit.Framework;

namespace Tests;

public class Sha256Test {
    [SetUp]
    public void Setup() { }

    [Test]
    [TestCase("halo dunia", "CbULL2heeng+7uAtgHDmcNSK8RiksA97/r6fjw5dPVI=")]
    [TestCase("coba lagi", "uC0s/5+tL6tiMglHMkoeZodPnJUq9J/O7iajOvAo9lY=")]
    public void Sha256FromStringTest(string input, string output) {
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

    [Test]
    [TestCase("halo dunia", "CbULL2heeng+7uAtgHDmcNSK8RiksA97/r6fjw5dPVI=")]
    [TestCase("coba lagi", "uC0s/5+tL6tiMglHMkoeZodPnJUq9J/O7iajOvAo9lY=")]
    public void Sha256FromBytesTest(string input, string output) {
        // var source = "halo dunia";
        string reference;
        using (var sha256 = SHA256.Create()) {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashed = sha256.ComputeHash(bytes);
            reference = Convert.ToBase64String(hashed);
            Assert.AreEqual(reference, output);
        }

        var inputBytes = Encoding.UTF8.GetBytes(input);
        var result = inputBytes.AsSpan().ToSha256Base64String();
        Assert.AreEqual(result, reference);
    }

    [Test]
    [TestCase("halo dunia", "CbULL2heeng.7uAtgHDmcNSK8RiksA97_r6fjw5dPVI")]
    [TestCase("coba lagi", "uC0s_5.tL6tiMglHMkoeZodPnJUq9J_O7iajOvAo9lY")]
    public void Sha256UrlSafeTest(string input, string output) {
        // var source = "halo dunia";
        // string reference;
        // using (var sha256 = SHA256.Create()) {
        //     var bytes = Encoding.UTF8.GetBytes(input);
        //     var hashed = sha256.ComputeHash(bytes);
        //     reference = Convert.ToBase64String(hashed);
        //     // Assert.AreEqual(reference, output);
        // }
        var result = input.ToSha256Base64String(true);
        Assert.AreEqual(result, output);
    }
}
