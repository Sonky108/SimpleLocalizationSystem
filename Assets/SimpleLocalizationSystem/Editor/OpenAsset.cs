using UnityEditor;
using UnityEditor.Callbacks;

namespace SimpleLocalizationSystem.Editor
{
	public static class OpenAsset
	{
		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			string assetPath = AssetDatabase.GetAssetPath(instanceID);
			SimpleLocalizationSystem scriptableObject = AssetDatabase.LoadAssetAtPath<SimpleLocalizationSystem>(assetPath);

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