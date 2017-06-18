using System;
using System.Collections.Generic;
using System.Text;

namespace VSLee.Utils.Text
{
	public static partial class TextExtensionMethods
    {
		public static List<string> SplitCSV(this string input)
		{
			return new List<string>(input.Split(new char[] { ' ', ',', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries));
		}

		/// <summary>
		/// @ is a verbatim string, where escape \ is not needed
		/// </summary>
		private static System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"\\");

		public static string ReplaceChars(this string input)
		{
			// first, convert it to lowercase
			input = input.ToLowerInvariant();
			// then... (do who knows what...)
			string replacement = ".";
			return rgx.Replace(input, replacement);
		}
	}
}
