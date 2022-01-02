using System.Collections.Generic;
using SimpleLocalizationSystem.Common;
using SimpleLocalizationSystem.Editor.ProjectSettings.Drawers;
using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor.ProjectSettings
{
	public static class SLSIMGUIRegister
	{
		private static readonly Dictionary<Storage, INestedDrawer> _customStorageDrawers = new Dictionary<Storage, INestedDrawer>
		{
			{Storage.Local, new LocalStorageDrawer()}, {Storage.Remote, new RemoteStorageDrawer()}
		};

		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			// First parameter is the path in the Settings window.
			// Second parameter is the scope of this setting: it only appears in the Project Settings window.
			SettingsProvider provider = new SettingsProvider("Project/MyCustomIMGUISettings", SettingsScope.Project)
			{
				// By default the last token of the path is used as display name if no label is provided.
				label = "Simple Localization System",
				// Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
				guiHandler = searchContext =>
				             {
					             SerializedObject settings = Settings.GetSerializedSettings();
					             Settings target = (Settings) settings.targetObject;
					             SerializedProperty serializedStorageProperty = settings.FindProperty("Storage");
					             EditorGUILayout.PropertyField(serializedStorageProperty, new GUIContent("Storage"));

					             _customStorageDrawers[(Storage) serializedStorageProperty.enumValueIndex].Draw(settings);
					             
					             EditorGUILayout.PropertyField(settings.FindProperty("LocaleFilePrefix"), new GUIContent("Locale file prefix"));

					             settings.ApplyModifiedProperties();
				             },

				// Populate the search keywords to enable smart search filtering and label highlighting:
				keywords = new HashSet<string>(new[] {"Number", "Serialization type"})
			};

			return provider;
		}
	}
}