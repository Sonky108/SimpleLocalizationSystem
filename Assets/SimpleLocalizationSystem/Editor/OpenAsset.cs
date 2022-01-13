using UnityEditor;
using UnityEditor.Callbacks;

namespace SimpleLocalizationSystem.Editor
{
	public static class OpenAsset
	{
		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			SimpleLocalizationSystem scriptableObject = EditorUtility.InstanceIDToObject(instanceID) as SimpleLocalizationSystem;

			if (scriptableObject != null)
			{
				Backend newBackend = new Backend(instanceID);
				Window window = EditorWindow.GetWindow<Window>();
				window.Backend = newBackend;
				window.Show();
				window.Start();
				return true;
			}
			
			return false;
		}
	}
}