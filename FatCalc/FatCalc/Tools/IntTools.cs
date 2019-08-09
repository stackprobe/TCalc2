using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public static class IntTools
	{
		public const int IMAX = 1000000000; // 10^9

		public static int ToInt(double value)
		{
			if (value < 0.0)
				return (int)(value - 0.5);
			else
				return (int)(value + 0.5);
		}

		public static int ToInt(string str, int minval, int maxval, int defval)
		{
			try
			{
				int value = int.Parse(str);

				if (IsRange(value, minval, maxval))
				{
					return value;
				}
			}
			catch
			{ }

			return defval;
		}

		public static bool IsRange(int value, int minval = 0, int maxval = IMAX)
		{
			return minval <= value && value <= maxval;
		}
	}
}
