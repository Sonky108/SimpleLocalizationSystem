using UnityEditor;

namespace SimpleLocalizationSystem.Editor.ProjectSettings.Drawers
{
	public interface INestedDrawer
	{
		void Draw(SerializedObject serializedObject);
	}
}