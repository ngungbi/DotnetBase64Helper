using System.Diagnostics;
using System.Security.Cryptography;

namespace Ngb.Base64Helper.Sha512; 

public static class Sha512Extensions {
    public static string ToSha512Base64String(this Span<byte> source, bool uriSafe = false) {
        // using var sha512 = SHA512.Create();
        Span<byte> sha512Span = stackalloc byte[64];

        if (!SHA512.TryHashData(source, sha512Span, out _)) {
            throw new InvalidOperationException();
        }

        Span<char> base64Span = stackalloc char[88];
        if (!Convert.TryToBase64Chars(sha512Span, base64Span, out _)) {
            throw new InvalidOperationException();
        }

        if (!uriSafe) return new string(base64Span);

        MakeUriSafe(base64Span);

        return new string(base64Span[..86]);
    }


    public static string ToSha512Base64String(this string source, bool uriSafe = false) {
        var length = source.Length;
        // using var sha256 = SHA256.Create();
        // Span<byte> sha256Span = stackalloc byte[32];
        Span<byte> byteSpan = stackalloc byte[length];
        var sourceSpan = source.AsSpan();
        CopyToBytes(sourceSpan, byteSpan);

        return ToSha512Base64String(byteSpan, uriSafe);
        
    }

    private static void CopyToBytes(ReadOnlySpan<char> source, Span<byte> destination) {
        Debug.Assert(source.Length <= destination.Length);
        for (int i = 0; i < source.Length; i++) {
            destination[i] = (byte) source[i];
        }
    }

    private static void MakeUriSafe(Span<char> chars) {
        for (int i = 0; i < chars.Length; i++) {
            chars[i] = chars[i]switch {
                // '=' => '-',
                '+' => '.',
                '/' => '_',
                _ => chars[i]
            };
        }
    }
}
