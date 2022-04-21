namespace Base64;

public static class Base64Extensions {
    private const char PaddingSign = '=';

    public static string? ToBase64String(this byte[] source, bool trimPadding = false) {
        var length = source.Length;
        Span<char> output = stackalloc char[length * 2];
        if (!Convert.TryToBase64Chars(source, output, out var count)) return null;

        if (trimPadding) {
            var equalPosition = output.IndexOf(PaddingSign);
            if (equalPosition > 0) {
                return new string(output[..equalPosition]);
            }
        }

        return new string(output[..count]);
    }

    public static string? ToBase64String(this string source, bool trimPadding = false) {
        var length = source.Length;
        Span<char> output = stackalloc char[length * 2];
        Span<byte> byteSpan = stackalloc byte[length];

        for (int i = 0; i < length; i++) {
            byteSpan[i] = (byte) source[i];
        }

        if (!Convert.TryToBase64Chars(byteSpan, output, out var count)) return null;
        if (trimPadding) {
            var equalPosition = output.IndexOf(PaddingSign);
            if (equalPosition > 0) {
                return new string(output[..equalPosition]);
            }
        }

        return new string(output[..count]);
    }

    public static string? ToStringFromBase64(this string source, bool addPadding = false) {
        var length = source.Length;
        Span<byte> output = stackalloc byte[length];
        int writeCount;
        if (addPadding) {
            var paddingCount = (length % 4) switch {
                2 => 2,
                3 => 1,
                _ => 0
            };
            var totalCount = length + paddingCount;
            Span<char> paddedSource = stackalloc char[totalCount];
            for (int i = 0; i < length; i++) {
                paddedSource[i] = source[i];
            }

            for (int i = length; i < totalCount; i++) {
                paddedSource[i] = PaddingSign;
            }

            if (!Convert.TryFromBase64Chars(paddedSource, output, out writeCount)) return null;
        } else {
            if (!Convert.TryFromBase64Chars(source, output, out writeCount)) return null;
        }

        Span<char> outputChars = stackalloc char[writeCount];
        for (int i = 0; i < writeCount; i++) {
            outputChars[i] = (char) output[i];
        }

        return new string(outputChars);
    }

    public static byte[]? ToByteArrayFromBase64(this string source, bool addPadding = false) {
        var length = source.Length;
        Span<byte> output = stackalloc byte[length];
        int writeCount;
        if (addPadding) {
            var paddingCount = (length % 4) switch {
                2 => 2,
                3 => 1,
                _ => 0
            };
            var totalCount = length + paddingCount;
            Span<char> paddedSource = stackalloc char[totalCount];
            for (int i = 0; i < length; i++) {
                paddedSource[i] = source[i];
            }

            for (int i = length; i < totalCount; i++) {
                paddedSource[i] = PaddingSign;
            }

            if (!Convert.TryFromBase64Chars(paddedSource, output, out writeCount)) return null;
        } else {
            if (!Convert.TryFromBase64Chars(source, output, out writeCount)) return null;
        }

        return output[..writeCount].ToArray();
    }
}
