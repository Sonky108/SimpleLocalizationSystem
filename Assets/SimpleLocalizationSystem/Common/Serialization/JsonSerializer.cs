using Newtonsoft.Json;
using SimpleLocalizationSystem.Common.Data;

namespace SimpleLocalizationSystem.Common.Serialization
{
	public class JsonSerializer : ILocaleSerializer
	{
		public LocalizationData Deserialize(string text)
		{
			return JsonConvert.DeserializeObject<LocalizationData>(text);
		}

		public string Serialize(LocalizationData data)
		{
			return JsonConvert.SerializeObject(data, Formatting.Indented);
		}
	}
}