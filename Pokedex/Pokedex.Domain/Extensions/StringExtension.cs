using System.Text.RegularExpressions;

namespace Pokedex.Domain.Extensions
{
	public static class StringExtension
	{
		/// <summary>
		/// RemoveEscapeChars
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static string RemoveEscapeChars(this string strValue)
		{
			var rgx4 = new Regex(@"[\0\a\b\f\n\r\t\v'/\\]");
			strValue = rgx4.Replace(strValue, " ");
			return strValue;
		}
	}
}
