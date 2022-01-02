using System;
using System.Globalization;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Common.Serialization;
using SimpleLocalizationSystem.Runtime.Loader;
using UnityEngine;

namespace SimpleLocalizationSystem.Runtime
{
	public class LocalizationManager : ILocalizationProvider
	{
		public ILocaleLoader LocaleLoader;
		private CultureInfo _currentLocale;
		private readonly ILocaleSerializer _serializer;
		private LocalizationData _currentLocalization;

		public LocalizationManager()
		{
			LocaleLoader = new StreamingAssetsLoader();
			_serializer = new JsonSerializer();
			SetLanguage(new CultureInfo("en-US"));
		}

		public event Action LocaleChanged;

		public string GetTranslation(string key)
		{
			if (_currentLocalization.Data.ContainsKey(key))
			{
				return _currentLocalization.Data[key];
			}

			Debug.LogWarning($"Cannot find translation for key {key}");
			
			return key;
		}

		public void SetLanguage(CultureInfo cultureInfo)
		{
			_currentLocale = cultureInfo;
			LocaleLoader.LoadLocale("localeEN.json", data =>
			                                         {
				                                         _currentLocalization = data;
				                                         LocaleChanged?.Invoke();
			                                         }, _serializer);
		}
	}
}