using System.Collections;
using UnityEngine;

namespace SimpleLocalizationSystem.Runtime.Utils
{
	public class CoroutineRunner : MonoBehaviour
	{
		private static CoroutineRunner _instance;
		private static CoroutineRunner Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject().AddComponent<CoroutineRunner>();
					DontDestroyOnLoad(_instance);
				}

				return _instance;
			}
		}

		public static void Run(IEnumerator coroutine)
		{
			Instance.StartCoroutine(coroutine);
		}
	}
}