using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SimpleLocalizationSystem.Common;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Editor.ProjectSettings;
using UnityEditor;

namespace SimpleLocalizationSystem.Editor
{
	public class Backend
	{
		public readonly Dictionary<CultureInfo, string> _localeFileByCultureInfo = new Dictionary<CultureInfo, string>();
		public List<string> Keys = new List<string>();
		public List<CultureInfo> Languages = new List<CultureInfo>();
		public Dictionary<CultureInfo, LocalizationData> Data = new Dictionary<CultureInfo, LocalizationData>();

		public Backend(int instanceID)
		{
			Keys.Add("NO_MSG");
			Keys.Add("YES_MSG");
			Keys.Add("MSG");
			string assetPath = AssetDatabase.GetAssetPath(instanceID);
			SimpleLocalizationSystem scriptableObject = AssetDatabase.LoadAssetAtPath<SimpleLocalizationSystem>(assetPath);

			if (scriptableObject != null)
			{
				_localeFileByCultureInfo.Clear();

				foreach (string x in Directory.GetFiles(Settings.Get().GetSavePath()))
				{
					if (x.Contains(Settings.Get().LocaleFilePrefix) && Path.GetExtension(x) == CommonUtils.GetLocaleExtension(Settings.LocalSerializationType))
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(x);
						CultureInfo cultureInfo = new CultureInfo(fileNameWithoutExtension.Substring(fileNameWithoutExtension.Length - 2, 2));
						_localeFileByCultureInfo.Add(cultureInfo, x);
						Languages.Add(cultureInfo);
						LoadLocale(cultureInfo);
					}
				}
			}
		}

		private Settings Settings => Settings.Get();
		public int LocaleFilesCount => _localeFileByCultureInfo.Count;

		public void LoadLocale(CultureInfo cultureInfo)
		{
			var serializer = CommonUtils.GetSerializer(Settings.LocalSerializationType);
			Data.Add(cultureInfo, serializer.Deserialize(File.ReadAllText(_localeFileByCultureInfo[cultureInfo])));
		}
	}
}