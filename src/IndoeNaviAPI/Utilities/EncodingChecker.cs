namespace IndoeNaviAPI.Utilities;

public abstract partial class EncodingChecker
{
	public static bool IsBase64String(string base64)
	{
		Span<byte> buffer = new(new byte[base64.Length]);
		return Convert.TryFromBase64String(base64, buffer, out _);
	}
}
