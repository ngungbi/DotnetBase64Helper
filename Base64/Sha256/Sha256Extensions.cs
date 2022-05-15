using System.Security.Cryptography;

namespace Ngb.Base64Helper.Sha256;

public static class Sha256Extensions {
    public static string? ToSha256Base64String(this string source, bool uriSafe = false) {
        var length = source.Length;
        using var sha256 = SHA256.Create();
        Span<byte> sha256Span = stackalloc byte[32];
        Span<byte> byteSpan = stackalloc byte[length];
        for (int i = 0; i < length; i++) {
            byteSpan[i] = (byte) source[i];
        }

        if (!sha256.TryComputeHash(byteSpan, sha256Span, out _)) return null;
        Span<char> base64Span = stackalloc char[44];
        if (!Convert.TryToBase64Chars(sha256Span, base64Span, out _)) return null;
        if (!uriSafe) return new string(base64Span);

        for (int i = 0; i < 44; i++) {
            base64Span[i] = base64Span[i] switch {
                // '=' => '-',
                '+' => '.',
                '/' => '_',
                _ => base64Span[i]
            };
        }

        return new string(base64Span[..43]);
    }
}
