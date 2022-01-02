using System;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Common.Serialization;

namespace SimpleLocalizationSystem.Runtime.Loader
{
	public interface ILocaleLoader
	{
		void LoadLocale(string fileLocalPath, Action<LocalizationData> localeLoaded, ILocaleSerializer serializer);
		void StopLoading();
	}
}