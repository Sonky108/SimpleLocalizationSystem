using System;

namespace SimpleLocalizationSystem.Runtime
{
	public interface ILocalizationProvider
	{
		string GetTranslation(string key);
		event Action LocaleChanged;
	}
}