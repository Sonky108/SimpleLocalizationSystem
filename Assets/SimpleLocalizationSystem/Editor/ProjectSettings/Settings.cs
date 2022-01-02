using System;
using SimpleLocalizationSystem.Common;
using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor.ProjectSettings
{
	internal class Settings : ScriptableObject
	{
		private const string SETTINGS_PATH = "Assets/SimpleLocalizationSystem/Editor/Settings.asset";
		private static Settings _settings;
		public Storage Storage;
		public LocalSerializationType LocalSerializationType;
		public RemoteStorageProvider RemoteStorageProvider;
		public SavePath SavePath;
		public string CustomPath;
		public string RemotePath;
		public string LocaleFilePrefix;

		public static Settings Get()
		{
			if (_settings == null)
			{
				_settings = AssetDatabase.LoadAssetAtPath<Settings>(SETTINGS_PATH);

				if (_settings == null)
				{
					_settings = CreateInstance<Settings>();
					AssetDatabase.CreateAsset(_settings, SETTINGS_PATH);
					AssetDatabase.SaveAssets();
				}
			}

			return _settings;
		}

		internal static SerializedObject GetSerializedSettings()
		{
			return new SerializedObject(Get());
		}

		public string GetSavePath()
		{
			switch (SavePath)
			{
				case SavePath.StreamingAssetsPath:
					return Application.streamingAssetsPath;
				case SavePath.Custom:
					return CustomPath;
				case SavePath.PersistentDataPath:
					return Application.persistentDataPath;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}