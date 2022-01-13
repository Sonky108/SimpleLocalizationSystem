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
		private const string SLS_LAST_OPENED_ASSET_KEY = "SLSLastOpenedAsset";

		public List<string> Keys = new List<string>();
		public List<CultureInfo> Languages => _simpleLocalizationSystem.Data.Keys.ToList();

		public Dictionary<CultureInfo, LocalizationData> Data => _simpleLocalizationSystem.Data;
		private readonly SimpleLocalizationSystem _simpleLocalizationSystem;

		public Backend(int instanceID)
		{
			Keys.Clear();

			string assetPath = AssetDatabase.GetAssetPath(instanceID);
			_simpleLocalizationSystem = AssetDatabase.LoadAssetAtPath<SimpleLocalizationSystem>(assetPath);

			EditorPrefs.SetString(SLS_LAST_OPENED_ASSET_KEY, AssetDatabase.AssetPathToGUID(assetPath));
			
			if (_simpleLocalizationSystem == null)
			{
				throw new MissingReferenceException($"Somehow cannot find SimpleLocalizationSystem asset with instance id = {instanceID}!");
			}

			FillKeys();
		}

		private Backend(string guid)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			_simpleLocalizationSystem = AssetDatabase.LoadAssetAtPath<SimpleLocalizationSystem>(assetPath);
			
			Keys.Clear();

			
			if (_simpleLocalizationSystem == null)
			{
				throw new MissingReferenceException($"Somehow cannot find SimpleLocalizationSystem asset with instance guid = {guid}!");
			}

			FillKeys();
		}
		
		public Backend(SimpleLocalizationSystem asset)
		{
			Keys.Clear();

			_simpleLocalizationSystem = asset;
			
			FillKeys();
		}

		private void FillKeys()
		{
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
				return true;
			}
			
			Error?.Invoke(resultCode);
			return false;
		}

		public void Clear()
		{
			EditorPrefs.DeleteKey(SLS_LAST_OPENED_ASSET_KEY);
		}

		public static bool CanRestore()
		{
			return EditorPrefs.HasKey(SLS_LAST_OPENED_ASSET_KEY);
		}

		public static Backend Restore()
		{
			return new Backend(EditorPrefs.GetString(SLS_LAST_OPENED_ASSET_KEY));
		}
	}
}