using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.CalcUtilsD
{
	public class CalcD
	{
		private int Radix;
		private int Basement;

		public CalcD()
			: this(10)
		{ }

		public CalcD(int radix)
			: this(radix, 30)
		{ }

		public CalcD(int radix, int basement)
		{
			if (radix < 2 || IntTools.IMAX < radix)
				throw new ArgumentException("Bad radix: " + radix);

			if (basement < 0 || IntTools.IMAX < basement)
				throw new ArgumentException("Bad basement: " + basement);

			this.Radix = radix;
			this.Basement = basement;
		}

		public string Calc(string leftOperand, string operation, string rightOperand)
		{
			int divBasement = 0;

			if (operation == "/")
				divBasement = this.Basement;

			FatConverter conv = new FatConverter(this.Radix);

			BusyDlg.StatusBox.Post("計算可能な形式を取得しています。(CalcD)"); // app

			conv.SetString(leftOperand);
			FatFloat a = conv.GetFloat();
			conv.SetString(rightOperand);
			conv.Exponent -= divBasement;
			FatFloat b = conv.GetFloat();

			BusyDlg.StatusBox.Post("計算しています。(CalcD)"); // app

			FatFloat ans = this.CalcMain(a, operation, b, conv.Rdx);

			BusyDlg.StatusBox.Post("計算可能な形式をセットしています。(CalcD)"); // app

			conv.SetFloat(ans);
			conv.Exponent -= divBasement;
			string answer = conv.GetString(this.Basement);

			return answer;
		}

		private FatFloat CalcMain(FatFloat a, string operation, FatFloat b, int radix)
		{
			if (operation == "+")
			{
				new FatFloatCalc(radix).Add(a, b);
				return a;
			}
			if (operation == "-")
			{
				new FatFloatCalc(radix).Sub(a, b);
				return a;
			}
			if (operation == "*")
			{
				return new FatFloatCalc(radix).Mul(a, b);
			}
			if (operation == "/")
			{
				return new FatFloatCalc(radix).Div(a, b);
			}
			throw new ArgumentException("Bad operator: " + operation);
		}

		public string Power(string operand, int exponent)
		{
			if (exponent < 0)
				throw new ArgumentException("Bad exponent: " + exponent);

			FatConverter conv = new FatConverter(this.Radix);

			BusyDlg.StatusBox.Post("計算可能な形式を取得しています。(CalcD.Power)"); // app

			conv.SetString(operand);
			FatFloat a = conv.GetFloat();

			BusyDlg.StatusBox.Post("計算しています。(CalcD.Power)"); // app

			FatFloat ans = this.PowerMain(a, exponent, conv.Rdx);

			BusyDlg.StatusBox.Post("計算可能な形式をセットしています。(CalcD.Power)"); // app

			conv.SetFloat(ans);
			string answer = conv.GetString(-1);

			return answer;
		}

		private FatFloat PowerMain(FatFloat a, int exponent, int radix)
		{
			if (exponent == 0)
				return new FatFloat(new FatUFloat(new FatUInt(new int[] { 1 }), 0), 1);

			if (exponent == 1)
				return a;

			if (exponent == 2)
				return new FatFloatCalc(radix).Mul(a, a);

			{
				FatFloat ans = this.PowerMain(a, exponent / 2, radix);

				if (exponent % 2 == 1)
					ans = new FatFloatCalc(radix).Mul(ans, new FatFloatCalc(radix).Mul(ans, a));
				else
					ans = new FatFloatCalc(radix).Mul(ans, ans);

				return ans;
			}
		}
	}
}
