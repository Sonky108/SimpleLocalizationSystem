using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor.ProjectSettings.Drawers
{
	internal class GoogleProviderDrawer : INestedDrawer
	{
		public void Draw(SerializedObject serializedObject)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RemotePath"), new GUIContent("Address"));
		}
	}
}