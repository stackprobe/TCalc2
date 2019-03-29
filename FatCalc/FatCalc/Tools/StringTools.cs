using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class StringTools
	{
		public const string ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public const string alpha = "abcdefghijklmnopqrstuvwxyz";
		public const string DECIMAL = "0123456789";
		public const string HEXADECIMAL = "0123456789ABCDEF";
		public const string hexadecimal = "0123456789abcdef";

		public static readonly Encoding ENCODING_SJIS = Encoding.GetEncoding(932);

		public static string Remove(string str, int offset, int count)
		{
			List<char> tmp = ToCharList(str);
			tmp.RemoveRange(offset, count);
			return ToString(tmp);
		}

		public static List<char> ToCharList(string src)
		{
			return src.ToCharArray().ToList();
		}

		public static string ToString(List<char> src)
		{
			return new string(src.ToArray());
		}

		public static string ReplaceChar(string str, string rChrs, char wChr)
		{
			foreach (char rChr in rChrs)
			{
				str = str.Replace(rChr, wChr);
			}
			return str;
		}

		public static string ReplaceLoop(string str, string rPtn, string wPtn, int count = 20)
		{
			while (0 < count)
			{
				str = str.Replace(rPtn, wPtn);
				count--;
			}
			return str;
		}

		public const string S_TRUE = "true";
		public const string S_FALSE = "false";

		public static bool ToFlag(string str)
		{
			return Equals(str, S_TRUE, true);
		}

		public static bool Equals(string str1, string str2, bool ignoreCase = false)
		{
			if (ignoreCase)
			{
				str1 = str1.ToLower();
				str2 = str2.ToLower();
			}
			return str1 == str2;
		}

		public static string Reverse(string src)
		{
			StringBuilder buff = new StringBuilder();

			for (int index = src.Length - 1; 0 <= index; index--)
			{
				buff.Append(src[index]);
			}
			return buff.ToString();
		}
	}
}
