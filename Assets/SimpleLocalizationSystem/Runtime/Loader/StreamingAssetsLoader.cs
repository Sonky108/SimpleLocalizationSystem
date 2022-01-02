using System;
using System.Collections;
using System.IO;
using SimpleLocalizationSystem.Common.Data;
using SimpleLocalizationSystem.Common.Serialization;
using SimpleLocalizationSystem.Runtime.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace SimpleLocalizationSystem.Runtime.Loader
{
	public class StreamingAssetsLoader : ILocaleLoader
	{
		private UnityWebRequest _webRequest;

		public void LoadLocale(string fileLocalPath, Action<LocalizationData> localeLoaded, ILocaleSerializer serializer)
		{
			CoroutineRunner.Run(Load(fileLocalPath, localeLoaded, serializer));
		}

		public void StopLoading()
		{
			_webRequest.Abort();
		}

		private IEnumerator Load(string fileLocalPath, Action<LocalizationData> localeLoaded, ILocaleSerializer serializer)
		{
			using (_webRequest = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, fileLocalPath)))
			{
				yield return _webRequest.SendWebRequest();

				if (_webRequest.isNetworkError || _webRequest.isHttpError)
				{
					Debug.Log($"Error loading file at path {fileLocalPath}");
				}
				else
				{
					localeLoaded?.Invoke(serializer.Deserialize(_webRequest.downloadHandler.text));
					Debug.Log(_webRequest.downloadHandler.text);
				}
			}
		}
	}
}