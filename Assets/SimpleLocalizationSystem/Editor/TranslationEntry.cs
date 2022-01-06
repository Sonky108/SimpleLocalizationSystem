using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	[Serializable]
	public class TranslationEntry
	{
		public string Language;
		[SerializeField] public List<TranslationData> Data;
	}
}