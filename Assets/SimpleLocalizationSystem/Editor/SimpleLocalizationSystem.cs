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
	public class SimpleLocalizationSystem : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField] private List<TranslationEntry> _serializedData = new List<TranslationEntry>();
		public Dictionary<CultureInfo, LocalizationData> Data = new Dictionary<CultureInfo, LocalizationData>();

		public void OnBeforeSerialize()
		{
			foreach (var x in Data)
			{
				if (_serializedData.Count != Data.Count && _serializedData.Count(y => y.Language == x.Key.TwoLetterISOLanguageName) == 0)
				{
					var data = new List<TranslationData>();

					foreach (var key in x.Value.Data)
					{
						data.Add(new TranslationData {Key = key.Key, Value = key.Value,});
					}

					_serializedData.Add(new TranslationEntry {Data = data, Language = x.Key.TwoLetterISOLanguageName});
				}

				for (int i = _serializedData.Count - 1; i >= 0; i--)
				{
					TranslationEntry y = _serializedData[i];

					if (y.Language == x.Key.TwoLetterISOLanguageName)
					{
						foreach (var key in x.Value.Data)
						{
							TranslationData entry = y.Data.FirstOrDefault(z => z.Key == key.Key);

							if (entry == null)
							{
								y.Data.Add(new TranslationData {Key = key.Key, Value = key.Value,});
							}
							else
							{
								entry.Value = key.Value;
							}
						}
					}
				}
			}
		}

		public void OnAfterDeserialize()
		{
			Data.Clear();

			foreach (TranslationEntry x in _serializedData)
			{
				CultureInfo cultureInfo = new CultureInfo(x.Language);
				Data.Add(cultureInfo, new LocalizationData());

				foreach (TranslationData y in x.Data)
				{
					Data[cultureInfo].Data.Add(y.Key, y.Value);
				}
			}
		}

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
						stream.SetLength(0);
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

		public int ChangeKey(string newKey, string oldKey)
		{
			bool containsKey = Data.SelectMany(x => x.Value.Data).Count(y => y.Key == newKey) > 0;

			if (containsKey)
			{
				return Result.ERROR_KEY_ALREADY_EXIST;
			}

			foreach (var x in Data)
			{
				var translation = x.Value.Data[oldKey];
				x.Value.Data.Add(newKey, translation);
				x.Value.Data.Remove(oldKey);
			}

			return Result.OK;
		}

		public int DeleteKey(string key)
		{
			foreach (var x in Data)
			{
				x.Value.Data.Remove(key);
			}
			
			return Result.OK;
		}
		
		public int AddNewLanguage(string code)
		{
			CultureInfo cultureInfo;

			try
			{
				cultureInfo = new CultureInfo(code);
			}
			catch (CultureNotFoundException e)
			{
				return Result.ERROR_CULTURE_NOT_FOUND;
			}

			if (Data.ContainsKey(cultureInfo))
			{
				return Result.ERROR_LANGUAGE_ALREADY_EXISTS;
			}

			var data = new Dictionary<string, string>();

			if (Data.Count != 0)
			{
				foreach (var x in Data.First().Value.Data)
				{
					data.Add(x.Key, "");
				}
			}

			Data.Add(cultureInfo, new LocalizationData {Data = data});

			return Result.OK;
		}
	}
}