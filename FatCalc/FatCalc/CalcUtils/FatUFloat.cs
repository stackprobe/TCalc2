using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.CalcUtils
{
	public class FatUFloat
	{
		// 評価値 == _value / (_radix ^ _decimalPoint)

		private FatUInt _value;
		private UInt64 _radix; // 2 ～ 2^64-1
		private int _decimalPoint; // 0 ～ IMAX

		public FatUInt Value
		{
			get
			{
				return _value;
			}
		}

		public UInt64 Radix
		{
			get
			{
				return _radix;
			}
		}

		public int DecimalPoint
		{
			get
			{
				return _decimalPoint;
			}
		}

		public FatUFloat(FatUInt value, UInt64 radix, int decimalPoint)
		{
			if (value == null) throw new ArgumentException();
			if (radix < 2) throw new ArgumentOutOfRangeException();
			if (decimalPoint < 0 || IntTools.IMAX < decimalPoint) throw new ArgumentOutOfRangeException();

			_value = value;
			_radix = radix;
			_decimalPoint = decimalPoint;
		}

		public FatUFloat GetClone()
		{
			return new FatUFloat(_value.GetClone(), _radix, _decimalPoint);
		}

		public FatUFloat ChangeDecimalPoint(int decimalPointNew) // ret: .Value.Rem != null ... 丸め発生
		{
			if (decimalPointNew < 0 || IntTools.IMAX < decimalPointNew) throw new ArgumentOutOfRangeException();

			int e = decimalPointNew - _decimalPoint;
			FatUInt value;

			if (e < 0)
				value = FatUInt.Div(_value, FatUInt.Power(new FatUInt(_radix), -e));
			else if (0 < e)
				value = FatUInt.Mul(_value, FatUInt.Power(new FatUInt(_radix), e));
			else
				value = _value.GetClone();

			return new FatUFloat(value, _radix, decimalPointNew);
		}

		private static void Synchronize(ref FatUFloat a, ref FatUFloat b, int basement = 0)
		{
			if (a == null) throw new ArgumentException();
			if (b == null) throw new ArgumentException();
			if (a.Radix != b.Radix) throw new ArgumentException("diff radix");
			if (basement < 0 || IntTools.IMAX < basement) throw new ArgumentOutOfRangeException();

			int e = a.DecimalPoint - b.DecimalPoint - basement;

			if (e < 0)
				a = a.ChangeDecimalPoint(a.DecimalPoint - e);
			else if (0 < e)
				b = b.ChangeDecimalPoint(b.DecimalPoint + e);
		}

		public static FatUFloat Add(FatUFloat a, FatUFloat b)
		{
			Synchronize(ref a, ref b);
			return new FatUFloat(FatUInt.Add(a.Value, b.Value), a.Radix, a.DecimalPoint);
		}

		public static FatUFloat Red(FatUFloat a, FatUFloat b) // ret: null ... < 0
		{
			Synchronize(ref a, ref b);
			FatUInt value = FatUInt.Red(a.Value, b.Value);

			if (value == null)
				return null;

			return new FatUFloat(value, a.Radix, a.DecimalPoint);
		}

		public static FatUFloat Mul(FatUFloat a, FatUFloat b)
		{
			if (IntTools.IMAX < a.DecimalPoint + b.DecimalPoint) throw new ArgumentException();
			return new FatUFloat(FatUInt.Mul(a.Value, b.Value), a.Radix, a.DecimalPoint + b.DecimalPoint);
		}

		public static FatUFloat Div(FatUFloat a, FatUFloat b, int basement) // ret: .Value.Rem != null ... 丸め発生
		{
			Synchronize(ref a, ref b, basement);
			return new FatUFloat(FatUInt.Div(a.Value, b.Value), a.Radix, basement);
		}

		public static FatUFloat Mod(FatUFloat a, FatUFloat b, int basement)
		{
			if (a == null) throw new ArgumentException();
			if (b == null) throw new ArgumentException();
			if (basement < 0 || IntTools.IMAX < basement) throw new ArgumentOutOfRangeException();

			return Red(a, Mul(Div(a, b, basement), b));
		}

		public static FatUFloat Power(FatUFloat a, int exponent)
		{
			if (a == null) throw new ArgumentException();

			if (exponent == 0)
				return new FatUFloat(new FatUInt(1), a.Radix, 0);

			if (exponent < 1 || IntTools.IMAX / exponent < a.DecimalPoint) throw new ArgumentOutOfRangeException();

			return new FatUFloat(FatUInt.Power(a.Value, exponent), a.Radix, a.DecimalPoint * exponent);
		}

		public static FatUFloat Root(FatUFloat a, int exponent, int basement) // ret: .Value.Rem != null ... 丸め発生
		{
			if (a == null) throw new ArgumentException();
			if (exponent < 1 || IntTools.IMAX < exponent) throw new ArgumentOutOfRangeException();
			if (basement < 0 || IntTools.IMAX / exponent < basement) throw new ArgumentOutOfRangeException();

			FatUFloat ret = new FatUFloat(FatUInt.Root(a.ChangeDecimalPoint(exponent * basement).Value, exponent), a.Radix, basement);
			FatUFloat rem = Red(a, Power(ret, exponent));

			if (rem.Value.IsZero() == false)
				ret.Value.Rem = rem.Value;

			return ret;
		}

		public FatUFloat ChangeRadix(UInt64 radixNew, int basement) // ret: .Value.Rem != null ... 丸め発生
		{
			if (radixNew < 2) throw new ArgumentOutOfRangeException();
			if (basement < 0 || IntTools.IMAX < basement) throw new ArgumentOutOfRangeException();

			FatUInt value = _value;
			value = FatUInt.Mul(value, FatUInt.Power(new FatUInt(radixNew), basement));
			value = FatUInt.Div(value, FatUInt.Power(new FatUInt(_radix), _decimalPoint));

			return new FatUFloat(value, radixNew, basement);
		}

		public override string ToString()
		{
			return _value.ToString() + ":" + _radix + ":" + _decimalPoint;
		}
	}
}
