using System.Collections.Generic;
using System.Globalization;
using SimpleLocalizationSystem.Common.Data;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	[CreateAssetMenu(menuName = "Simple Localization System/Translation", fileName = "Simple Localization System")]
	public class SimpleLocalizationSystem : ScriptableObject
	{
		public Dictionary<CultureInfo, LocalizationData> Data;
	}
}