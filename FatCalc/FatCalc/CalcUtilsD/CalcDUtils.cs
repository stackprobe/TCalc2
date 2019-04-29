using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.CalcUtilsD
{
	public class CalcDUtils
	{
		public static string ApplyBracketMin(string operand, int bracketMin)
		{
			BusyDlg.StatusBox.Post("角括弧表記を適用しています。(CalcD)"); // app

			StringBuilder buff = new StringBuilder();

			for (int index = 0; index < operand.Length; index++)
			{
				char chr = operand[index];

				if (chr == '[')
				{
					for (; ; )
					{
						buff.Append(operand[index]);

						if (operand[index] == ']')
							break;

						index++;
					}
				}
				else
				{
					int value = FatConverter.DIGIT_36.IndexOf(chr);

					if (value < bracketMin)
						buff.Append(chr);
					else
						buff.Append("[" + value + "]");
				}
			}
			return buff.ToString();
		}
	}
}
