using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor.ProjectSettings.Drawers
{
	internal class CustomPathDrawer : INestedDrawer
	{
		public void Draw(SerializedObject serializedObject)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomPath"), new GUIContent("Custom path"));
		}
	}
}