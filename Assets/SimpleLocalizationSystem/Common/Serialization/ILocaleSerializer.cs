using SimpleLocalizationSystem.Common.Data;

namespace SimpleLocalizationSystem.Common.Serialization
{
	public interface ILocaleSerializer
	{
		LocalizationData Deserialize(string text);
		string Serialize(LocalizationData data);
	}
}