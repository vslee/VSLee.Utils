using System;
using System.Collections.Generic;
using System.Text;

namespace VSLee.Utils
{
    public static class RandomExtensionMethods
    {
		/// <summary>
		/// Returns an Int32 with a random value across the entire range of
		/// possible values.
		/// </summary>
		public static int NextInt32(this Random rng)
		{
			unchecked
			{
				int firstBits = rng.Next(0, 1 << 4) << 28;
				int lastBits = rng.Next(0, 1 << 28);
				return firstBits | lastBits;
			}
		}

		public static decimal NextDecimal(this Random rng)
		{ // https://stackoverflow.com/questions/609501/generating-a-random-decimal-in-c-sharp
			byte scale = (byte)rng.Next(29);
			bool sign = rng.Next(2) == 1;
			return new decimal(rng.NextInt32(),
							   rng.NextInt32(),
							   rng.NextInt32(),
							   sign,
							   scale);
		}

		/// <summary>
		/// generates a random number between the two decimal places specified
		/// </summary>
		/// <param name="rng"></param>
		/// <param name="decimalPlacesToStart">number of zeros prior to first number after decimal point</param>
		/// <param name="decimalPlacesToEnd">total number of digits after decimal point</param>
		/// <returns>can be a negative number</returns>
		public static decimal NextDecimal(this Random rng, int decimalPlacesToStart, int decimalPlacesToEnd)
		{
			var random = rng.RandomNumberBetween(minValue: -1, maxValue: 1);
			random /= (decimal)Math.Pow(10, Convert.ToDouble(decimalPlacesToStart));
			decimal roundMultiplier = (decimal)Math.Pow(10, Convert.ToDouble(decimalPlacesToEnd));
			return Math.Round(random * roundMultiplier) / roundMultiplier;
		}

		private static decimal RandomNumberBetween(this Random rng, double minValue, double maxValue)
		{ // https://stackoverflow.com/questions/17786771/random-double-between-given-numbers
			var next = rng.NextDouble();

			return new decimal(minValue + (next * (maxValue - minValue)));
		}

		/// <summary>
		/// For any positive number less than 1, returns number of zeros to the right of decimal point before first sig digit
		/// </summary>
		/// <param name="input"></param>
		/// <returns>number of 0s after decimal place, not negative</returns>
		public static int NumberOfDecimalPlaces(this decimal input)
		{
			var intermediary = input;
			int numberOfZeros = 0;
			while (intermediary < 1.0m)
			{
				intermediary *= 10;
				numberOfZeros++;
			}
			return numberOfZeros;
		}
	}
}
