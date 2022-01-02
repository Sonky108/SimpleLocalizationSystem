using System;
using SimpleLocalizationSystem.Common.Serialization;

namespace SimpleLocalizationSystem.Common
{
	public static class CommonUtils
	{
		public static ILocaleSerializer GetSerializer(LocalSerializationType type)
		{
			switch (type)
			{
				case LocalSerializationType.Json:
					return new JsonSerializer();
				case LocalSerializationType.XML:
					throw new NotImplementedException();
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public static string GetLocaleExtension(LocalSerializationType type)
		{
			switch (type)
			{
				case LocalSerializationType.Json:
					return ".json";
				case LocalSerializationType.XML:
					return ".xml";
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}