using UnityEditor;
using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	public interface IWindowSkin
	{
		Color DarkRowBackground { get; }
		Color LightRowBackground { get; }
		GUIStyle CellStyle { get; }
		GUIStyle BottomAreaStyle { get; }
		GUIContent AddButton { get; }
	}

	public class WindowSkin : IWindowSkin
	{
		public Color DarkRowBackground { get; } = Color.white * 0.1f;
		public Color LightRowBackground { get; } = Color.white * 0.3f;
		public GUIStyle CellStyle { get; } = new GUIStyle(GUI.skin.label) {padding = new RectOffset(10, 10, 2, 2)};
		public GUIStyle BottomAreaStyle { get; } = new GUIStyle(GUI.skin.textArea) {};
		public GUIContent AddButton { get; } = EditorGUIUtility.IconContent("d_Toolbar Plus", "| Add new key");
	}
}