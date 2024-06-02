using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteSaveLoadStrategy : ISaveLoadStrategy {
	public async Task Save(string data, string url) {
		await UploadFileAsync(data, url);
	}

	public async Task<string> Load(string url) {
		return await DownloadFileAsync(url);
	}

	private async Task UploadFileAsync(string data, string url) {
		using (UnityWebRequest www = UnityWebRequest.Put(url, data)) {
			var operation = www.SendWebRequest();

			while (!operation.isDone)
				await Task.Yield();

			if (www.result != UnityWebRequest.Result.Success) {
				Debug.LogError("Error uploading file: " + www.error);
			}
		}
	}
	//todo token
	private async Task<string> DownloadFileAsync(string url) {
		using (UnityWebRequest www = UnityWebRequest.Get(url)) {
			var operation = www.SendWebRequest();

			while (!operation.isDone)
				await Task.Yield();

			if (www.result == UnityWebRequest.Result.Success) {
				return www.downloadHandler.text;
			}
			else {
				Debug.LogError("Error downloading file: " + www.error);
				return null;
			}
		}
	}
}
