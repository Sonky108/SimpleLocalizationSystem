using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using SimpleLocalizationSystem.Common;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Common.Serialization;
using SimpleLocalizationSystem.Editor.ProjectSettings;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	[CreateAssetMenu(menuName = "Simple Localization System/Translation", fileName = "Simple Localization System")]
	public class SimpleLocalizationSystem : ScriptableObject
	{ 
		public Dictionary<CultureInfo, LocalizationData> Data = new Dictionary<CultureInfo, LocalizationData>();

		public void Export()
		{
			foreach (var x in Data)
			{
				Settings settings = Settings.Get();

				if (settings.Storage == Storage.Local)
				{
					ILocaleSerializer serializer = CommonUtils.GetSerializer(settings.LocalSerializationType);

					string serialized = serializer.Serialize(x.Value);

					using (FileStream stream =
						File.Open(Path.Combine(settings.GetSavePath(), $"{settings.LocaleFilePrefix}{x.Key.TwoLetterISOLanguageName}{CommonUtils.GetLocaleExtension(settings.LocalSerializationType)}"),
						          FileMode.OpenOrCreate))
					{
						byte[] data = new UTF8Encoding().GetBytes(serialized);
						stream.Write(data, 0, data.Length);
					}
				}
			}
		}

		public int TryAddNewKey(string key)
		{
			bool containsKey = Data.SelectMany(x => x.Value.Data).Count(y => y.Key == key) > 0;

			if (containsKey)
			{
				return Result.ERROR_KEY_ALREADY_EXIST;
			}

			foreach (var x in Data)
			{
				x.Value.Data.Add(key, "");
			}
			
			return Result.OK;
		}
	}
}