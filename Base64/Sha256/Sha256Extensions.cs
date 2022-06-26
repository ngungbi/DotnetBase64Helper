using System.Diagnostics;
using System.Security.Cryptography;

namespace Ngb.Base64Helper.Sha256;

public static class Sha256Extensions {
    public static string ToSha256Base64String(this Span<byte> source, bool uriSafe = false) {
        // using var sha256 = SHA256.Create();
        Span<byte> sha256Span = stackalloc byte[32];

        if (!SHA256.TryHashData(source, sha256Span, out _)) {
            throw new InvalidOperationException("Failed to create SHA256");
        }

        Span<char> base64Span = stackalloc char[44];
        if (!Convert.TryToBase64Chars(sha256Span, base64Span, out _)) {
            throw new InvalidOperationException("Failed to convert to base64");
        }

        if (!uriSafe) return new string(base64Span);

        MakeUriSafe(base64Span);

        return new string(base64Span[..43]);
    }

    public static string ToSha256Base64String(this ReadOnlySpan<char> source, bool uriSafe = false) {
        var length = source.Length;
        // using var sha256 = SHA256.Create();
        // Span<byte> sha256Span = stackalloc byte[32];
        Span<byte> byteSpan = stackalloc byte[length];
        // var sourceSpan = source.AsSpan();
        CopyToBytes(source, byteSpan);

        return ToSha256Base64String(byteSpan, uriSafe);
    }

    public static bool TryToSha256Base64Chars(this ReadOnlySpan<char> source, Span<char> destination, bool uriSafe = false) {
        var length = source.Length;
        Span<byte> byteSpan = stackalloc byte[length];
        CopyToBytes(source, byteSpan);
        return TryToSha256Base64Chars(byteSpan, destination, uriSafe);
    }

    public static bool TryToSha256Base64Chars(this Span<byte> source, Span<char> destination, bool uriSafe = false) {
        // var length = source.Length;
        // Span<byte> byteSpan = stackalloc byte[length];
        // CopyToBytes(source, byteSpan);
        Span<byte> sha256Bytes = stackalloc byte[32];
        if (!SHA256.TryHashData(source, sha256Bytes, out _)) return false;
        return Convert.TryToBase64Chars(sha256Bytes, destination, out _);
    }

    public static string ToSha256Base64String(this string source, bool uriSafe = false) {
        var length = source.Length;
        // using var sha256 = SHA256.Create();
        // Span<byte> sha256Span = stackalloc byte[32];
        Span<byte> byteSpan = stackalloc byte[length];
        var sourceSpan = source.AsSpan();
        CopyToBytes(sourceSpan, byteSpan);

        return ToSha256Base64String(byteSpan, uriSafe);
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
