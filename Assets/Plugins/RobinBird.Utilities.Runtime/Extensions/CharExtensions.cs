namespace RobinBird.Utilities.Runtime.Extensions
{
	public static class CharExtensions
	{
		public static bool IsAscii(this char c)
		{
			return c <= sbyte.MaxValue;
		}
	}
}