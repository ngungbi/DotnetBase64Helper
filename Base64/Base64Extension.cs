using System.Diagnostics;

namespace Ngb.Base64Helper;

public static class Base64Extensions {
    private const char PaddingSign = '=';

    /// <summary>
    /// Converts array of bytes to Base64 string.
    /// </summary>
    /// <param name="source">Source array of bytes</param>
    /// <param name="trimPadding">If set to true, will remove padding at the end</param>
    /// <returns>Returns Base64 string or null when failed</returns>
    public static string ToBase64String(this byte[] source, bool trimPadding = false) {
        return ToBase64String(source.AsSpan(), trimPadding);
    }

    /// <summary>
    /// Converts array of bytes to Base64 string.
    /// </summary>
    /// <param name="source">Source array of bytes</param>
    /// <param name="trimPadding">If set to true, will remove padding at the end</param>
    /// <returns>Returns Base64 string or null when failed</returns>
    public static string ToBase64String(this ReadOnlySpan<byte> source, bool trimPadding = false) {
        var length = source.Length;
        Span<char> output = stackalloc char[length * 2];
        if (!Convert.TryToBase64Chars(source, output, out var count)) {
            throw new InvalidOperationException();
        }

        if (trimPadding) {
            var paddingPosition = output.IndexOf(PaddingSign);
            if (paddingPosition > 0) {
                return new string(output[..paddingPosition]);
            }
        }

        return new string(output[..count]);
    }

    /// <summary>
    /// Converts string to Base64.
    /// </summary>
    /// <param name="source">Source string</param>
    /// <param name="trimPadding">If set to true, will remove padding at the end</param>
    /// <returns>Returns Base64 string or null when failed</returns>
    public static string ToBase64String(this string source, bool trimPadding = false) {
        var length = source.Length;
        Span<char> output = stackalloc char[length * 2];
        Span<byte> byteSpan = stackalloc byte[length];

        CopyToBytes(source.AsSpan(), byteSpan);

        if (!Convert.TryToBase64Chars(byteSpan, output, out var count)) {
            throw new InvalidOperationException();
        }

        if (trimPadding) {
            var equalPosition = output.IndexOf(PaddingSign);
            if (equalPosition > 0) {
                return new string(output[..equalPosition]);
            }
        }

        return new string(output[..count]);
    }

    [Obsolete($"Use {nameof(ToStringFromBase64)} without addPadding parameter")]
    public static string ToStringFromBase64(this string source, bool addPadding) => ToStringFromBase64(source);


    /// <summary>
    /// Converts Base64 string to normal string.
    /// </summary>
    /// <param name="source">Base64 string</param>
    /// <returns>Returns normal string or null when failed</returns>
    public static string ToStringFromBase64(this string source) {
        var length = source.Length;
        Span<byte> output = stackalloc byte[length];
        int writeCount;
        // if (addPadding) {
        var paddingCount = GetPaddingCount(length);
        if (paddingCount == 0) {
            if (!Convert.TryFromBase64Chars(source, output, out writeCount)) return string.Empty;
        } else {
            var totalCount = length + paddingCount;
            Span<char> paddedSource = stackalloc char[totalCount];
            source.CopyTo(paddedSource);
            FillPadding(paddedSource, length, totalCount);
            
            // if (paddingCount > 0) paddedSource[length..].Fill(PaddingSign);

            if (!Convert.TryFromBase64Chars(paddedSource, output, out writeCount)) return string.Empty;
        }

        Span<char> outputChars = stackalloc char[writeCount];
        // CopyToChars(output[..writeCount], outputChars);
        for (int i = 0; i < writeCount; i++) {
            outputChars[i] = (char) output[i];
        }

        return new string(outputChars);
    }

    /// <summary>
    /// Converts Base64 string to array of bytes.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="addPadding">If sets to true, will returns Base64 string with padding</param>
    /// <returns></returns>
    public static byte[] ToByteArrayFromBase64(this string source, bool addPadding = false) {
        var length = source.Length;
        Span<byte> output = stackalloc byte[length];
        int writeCount;
        if (addPadding) {
            var totalCount = length + GetPaddingCount(length);
            Span<char> paddedSource = stackalloc char[totalCount];
            source.CopyTo(paddedSource);
            FillPadding(paddedSource, length, totalCount);
            // paddedSource[length..].Fill(PaddingSign);

            if (!Convert.TryFromBase64Chars(paddedSource, output, out writeCount)) return Array.Empty<byte>();
        } else {
            if (!Convert.TryFromBase64Chars(source, output, out writeCount)) return Array.Empty<byte>();
        }

        return output[..writeCount].ToArray();
    }

    private static void FillPadding(Span<char> chars, int start, int end) {
        for (int i = start; i < end; i++) {
            chars[i] = PaddingSign;
        }
    }

    private static int GetPaddingCount(int length) {
        return (length % 4) switch {
            2 => 2,
            3 => 1,
            _ => 0
        };
    }

    private static void CopyToBytes(ReadOnlySpan<char> source, Span<byte> destination) {
        Debug.Assert(source.Length <= destination.Length);
        for (int i = 0; i < source.Length; i++) {
            destination[i] = (byte) source[i];
        }
    }

    private static void CopyToChars(ReadOnlySpan<byte> source, Span<char> destination) {
        Debug.Assert(source.Length <= destination.Length);
        for (int i = 0; i < source.Length; i++) {
            destination[i] = (char) source[i];
        }
    }
}
