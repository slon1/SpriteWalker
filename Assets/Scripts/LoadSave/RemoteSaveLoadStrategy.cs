using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// Provides a strategy for saving and loading data remote.
/// </summary>
public class RemoteSaveLoadStrategy : ISaveLoadStrategy {
	public async Task Save(string data, string url) {
		await UploadFileAsync(data, url);
	}

	public async Task<string> Load(string url) {
		return await DownloadFileAsync(url);
	}

	private async Task UploadFileAsync(string data, string url) {
		//Server.Put();
	}
	//todo token
	private async Task<string> DownloadFileAsync(string url) {
		//Server.Get();
		return null;
	}
}
