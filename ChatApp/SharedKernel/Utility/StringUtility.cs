namespace SharedKernel.Utility;

public static class StringUtility
{
    public static bool IsBase64(string input)
    {
        Span<byte> buffer = new(new byte[input.Length]);

        return Convert.TryFromBase64String(input, buffer, out _);
    }
}
