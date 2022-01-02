using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor.ProjectSettings.Drawers
{
	internal class LocalStorageDrawer : INestedDrawer
	{
		private static readonly Dictionary<SavePath, INestedDrawer> _drawers = new Dictionary<SavePath, INestedDrawer>
		{
			{SavePath.Custom, new CustomPathDrawer()}
		};

		public void Draw(SerializedObject serializedObject)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("LocalSerializationType"), new GUIContent("Serialization type"));

			SerializedProperty savePathProperty = serializedObject.FindProperty("SavePath");
			EditorGUILayout.PropertyField(savePathProperty, new GUIContent("Save path"));

			if (_drawers.ContainsKey((SavePath) savePathProperty.enumValueIndex))
			{
				_drawers[(SavePath) savePathProperty.enumValueIndex].Draw(serializedObject);
			}
		}
	}
}