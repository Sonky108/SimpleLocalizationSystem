using UnityEngine;

namespace SimpleLocalizationSystem.Editor
{
	public interface IWindowSkin
	{
		Color DarkRowBackground { get; }
		Color LightRowBackground { get; }
		GUIStyle CellStyle { get; }
	}

	public class WindowSkin : IWindowSkin
	{
		public Color DarkRowBackground { get; } = Color.white * 0.1f;
		public Color LightRowBackground { get; } = Color.white * 0.3f;
		public GUIStyle CellStyle { get; } = new GUIStyle(GUI.skin.label) {padding = new RectOffset(10, 10, 2, 2)};
	}
}