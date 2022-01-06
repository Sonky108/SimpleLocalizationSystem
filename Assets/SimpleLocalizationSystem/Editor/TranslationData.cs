using System;

namespace SimpleLocalizationSystem.Editor
{
	[Serializable]
	public class TranslationData
	{
		public string Key;
		public string Value;
		public override bool Equals(object obj)
		{
			if (obj is TranslationData data)
			{
				return Key == data.Key && Value == data.Value;
			}

			return false;
		}
	}
}