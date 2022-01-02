using System.Collections.Generic;
using SimpleLocalizationSystem.Common;
using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor.ProjectSettings.Drawers
{
	internal class RemoteStorageDrawer : INestedDrawer
	{
		private static readonly Dictionary<RemoteStorageProvider, INestedDrawer> _drawers = new Dictionary<RemoteStorageProvider, INestedDrawer>
		{
			{RemoteStorageProvider.Google, new GoogleProviderDrawer()}
		};

		public void Draw(SerializedObject serializedObject)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RemoteStorageProvider"), new GUIContent("Provider"));

			SerializedProperty remoteStorageProviderProperty = serializedObject.FindProperty("RemoteStorageProvider");
			_drawers[(RemoteStorageProvider) remoteStorageProviderProperty.enumValueIndex].Draw(serializedObject);
		}
	}
}