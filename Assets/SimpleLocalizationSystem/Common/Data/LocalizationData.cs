using System;
using System.Collections.Generic;

namespace SimpleLocalizationSystem.Common.Data
{
	[Serializable]
	public class LocalizationData
	{
		public Dictionary<string, string> Data = new Dictionary<string, string>();
	}
}