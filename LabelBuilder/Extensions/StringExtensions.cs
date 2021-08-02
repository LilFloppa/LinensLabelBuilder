namespace LabelBuilder.Extensions
{
	public static class StringExtensions
	{
		public static bool IsInt32(this string str) => int.TryParse(str, out _);
	}
}
