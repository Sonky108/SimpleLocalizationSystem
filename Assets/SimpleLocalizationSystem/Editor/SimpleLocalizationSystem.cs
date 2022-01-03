using System.Collections.Generic;
using System.Globalization;
using SimpleLocalizationSystem.Common.Data;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	[CreateAssetMenu(menuName = "Simple Localization System/Translation", fileName = "Simple Localization System")]
	public class SimpleLocalizationSystem : ScriptableObject
	{
		public Dictionary<CultureInfo, LocalizationData> Data = new Dictionary<CultureInfo, LocalizationData>() {{new CultureInfo("pl"), new LocalizationData()
		{
			Data = new Dictionary<string, string>()
			{
				{"NO_MSG", "Nie"},
				{"MSG", "Wiadomość"},
				{"YES_MSG", "Tak"},
				{"1", "Jeden"},
				{"2", "Dwa"},
				{"3", "Trzy"},
				{"4", "Cztery"},
				{"5", "Pięć"},
				{"6", "Sześć"},
				{"7", "Siedem"},
				{"8", "Osiem"},
				{"11", "Jeden"},
				{"21", "Dwa"},
				{"31", "Trzy"},
				{"41", "Cztery"},
				{"51", "Pięć"},
				{"61", "Sześć"},
				{"71", "Siedem"},
				{"81", "Osiem"},
			}
		}}};
	}
}