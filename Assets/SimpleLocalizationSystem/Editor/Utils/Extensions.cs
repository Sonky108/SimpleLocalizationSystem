using System;

namespace SimpleLocalizationSystem.Editor.Utils
{
	public static class Extensions
	{
		public static bool Contains(this string text, string value, StringComparison stringComparison)
		{
			return text.IndexOf(value, stringComparison) >= 0;
		}
	}
}