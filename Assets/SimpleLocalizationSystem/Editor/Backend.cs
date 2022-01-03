using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Editor.ProjectSettings;
using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	public class Backend
	{
		public List<string> Keys = new List<string>();
		public List<CultureInfo> Languages => _simpleLocalizationSystem.Data.Keys.ToList();

		public Dictionary<CultureInfo, LocalizationData> Data => _simpleLocalizationSystem.Data;
		private readonly SimpleLocalizationSystem _simpleLocalizationSystem;

		public Backend(int instanceID)
		{
			Keys.Clear();
			
			string assetPath = AssetDatabase.GetAssetPath(instanceID);
			_simpleLocalizationSystem = AssetDatabase.LoadAssetAtPath<SimpleLocalizationSystem>(assetPath);

			if (_simpleLocalizationSystem == null)
			{
				throw new MissingReferenceException($"Somehow cannot find SimpleLocalizationSystem asset with instance id = {instanceID}!");
			}

			foreach (var x in Data)
			{
				foreach (var y in x.Value.Data)
				{
					if (!Keys.Contains(y.Key))
					{
						Keys.Add(y.Key);
					}					
				}
			}
		}
	}
}