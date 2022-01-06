using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimpleLocalizationSystem.Common.Data;
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

		public void Export()
		{
			_simpleLocalizationSystem.Export();
			
		}

		public bool TryAddKey(string newKeyText)
		{
			int resultCode = _simpleLocalizationSystem.TryAddNewKey(newKeyText);

			if (Result.Succeeded(resultCode))
			{
				var ee = new SerializedObject(_simpleLocalizationSystem);
				ee.ApplyModifiedProperties();
				AssetDatabase.SaveAssets();
				Keys.Add(newKeyText);
				return true;
			}

			Error?.Invoke(resultCode);
			return false;
		}

		public event Action<int> Error;

		public bool AddNewLanguage(string code)
		{
			int resultCode = _simpleLocalizationSystem.AddNewLanguage(code);

			if (Result.Succeeded((resultCode)))
			{
				var ee = new SerializedObject(_simpleLocalizationSystem);
				ee.ApplyModifiedProperties();
				AssetDatabase.SaveAssets();
				return true;
			}
			
			Error?.Invoke(resultCode);
			return false;
		}
	}
}