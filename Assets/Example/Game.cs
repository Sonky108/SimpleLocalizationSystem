using System.Collections.Generic;
using System.IO;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Common.Serialization;
using SimpleLocalizationSystem.Runtime;
using UnityEngine;

namespace Example
{
	public class Game : MonoBehaviour
	{
		private ILocalizationProvider _localizationProvider;

		private void Awake()
		{
			LocalizationData data = new LocalizationData();
			data.Data = new Dictionary<string, string> {{"NO_MSG", "No"}, {"YES_MSG", "Yes"}, {"MSG", "Message"},};

			JsonSerializer goo = new JsonSerializer();

			File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "localeEN.json"), goo.Serialize(data));

			_localizationProvider = new LocalizationManager();
			_localizationProvider.LocaleChanged += () => { Debug.Log(_localizationProvider.GetTranslation("NO_MSG")); };
		}
	}
}