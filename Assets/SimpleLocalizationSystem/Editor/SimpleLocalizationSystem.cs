using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using SimpleLocalizationSystem.Common;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Common.Serialization;
using SimpleLocalizationSystem.Editor.ProjectSettings;
using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	[CreateAssetMenu(menuName = "Simple Localization System/Translation", fileName = "Simple Localization System")]
	public class SimpleLocalizationSystem : ScriptableObject, ISerializationCallbackReceiver
	{ 
		public Dictionary<CultureInfo, LocalizationData> Data = new Dictionary<CultureInfo, LocalizationData>();
		[SerializeField] private List<TranslationEntry> _serializedData = new List<TranslationEntry>();
		
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
			EditorUtility.SetDirty(this);
			
			return Result.OK;
		}

		public void OnBeforeSerialize()
		{
			Debug.Log("Before Serialize");

			foreach (var x in Data)
			{
				if (_serializedData.Count == 0)
				{
					var data = new List<TranslationData>();
						
					foreach(var key in x.Value.Data)
					{
						data.Add(new TranslationData()
						{
							Key = key.Key,
							Value = key.Value,
						});
					}
						
					_serializedData.Add(new TranslationEntry() {Data = data, Language = x.Key.TwoLetterISOLanguageName});
				}
				
				for (int i = _serializedData.Count - 1; i >= 0; i--)
				{
					var y = _serializedData[i];

					if (y.Language == x.Key.TwoLetterISOLanguageName)
					{
						foreach(var key in x.Value.Data)
						{
							var entry = y.Data.FirstOrDefault(z => z.Key == key.Key);
							
							if (entry == null)
							{
								y.Data.Add(new TranslationData()
								{
									Key = key.Key,
									Value = key.Value,
								});
							}
							else
							{
								entry.Value = key.Value;
							}
						}
					}
					else
					{
						
					}
				}
			}
		}

		public void OnAfterDeserialize()
		{
			Debug.Log("After Deserialize");
			Data.Clear();

			foreach (var x in _serializedData)
			{
				CultureInfo cultureInfo = new CultureInfo(x.Language);
				Data.Add(cultureInfo, new LocalizationData());

				foreach (var y in x.Data)
				{
					Data[cultureInfo].Data.Add(y.Key, y.Value);
				}
			}
		}

		public void AddNewLanguage(string code)
		{
			Data.Add(new CultureInfo(code), new LocalizationData());
		}
	}
}