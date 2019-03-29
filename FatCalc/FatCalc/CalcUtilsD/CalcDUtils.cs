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

			for (int index = 35; bracketMin <= index; index--)
			{
				operand = operand.Replace(FatConverter.DIGIT_36.Substring(index, 1), "[" + index + "]");
			}
			return operand;
		}
	}
}
