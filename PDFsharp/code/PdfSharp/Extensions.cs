using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSharp
{
	/// <summary>
	/// Extensions - common extension methods
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Is string null or empty
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool IsEmpty(this string val)
		{
			return string.IsNullOrEmpty(val);
		}

		/// <summary>
		/// Is string not null or empty
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool IsNotEmpty(this string val)
		{
			return !string.IsNullOrEmpty(val);
		}

		/// <summary>
		/// Is String equal to
		/// </summary>
		/// <param name="val1"></param>
		/// <param name="val2"></param>
		/// <returns></returns>
		public static bool IsEqual(this string val1, string val2)
		{
			return string.Compare(val1, val2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		/// <summary>
		/// is string not equal to
		/// </summary>
		/// <param name="val1"></param>
		/// <param name="val2"></param>
		/// <returns></returns>
		public static bool IsNotEqual(this string val1, string val2)
		{
			return !val1.IsEqual(val2);
		}

	}
}
