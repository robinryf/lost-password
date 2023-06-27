using System;

namespace RobinBird.Utilities.Runtime.Extensions
{
	public static class TimeSpanExtensions
	{
		/// <summary>
		/// Prints the time as short as possible with just showing the biggest time unit 
		/// </summary>
		public static string ToMinimalString(this TimeSpan span)
		{
			if (span.TotalHours > 1)
			{
				return $"{((int)span.TotalHours).ToString()}h";
			}
			if (span.TotalMinutes > 1)
			{
				return $"{((int)span.TotalMinutes).ToString()}m";
			}
			return $"{((int)span.TotalSeconds).ToString()}s";
		}
	}
}