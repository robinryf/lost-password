using System;

namespace RobinBird.Utilities.Runtime.Extensions
{
	public static class RandomExtensions
	{
		public static double NextDoubleRange(this Random random, double minNumber, double maxNumber)
		{
			return random.NextDouble() * (maxNumber - minNumber) + minNumber;
		}
	}
}