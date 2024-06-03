using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Server {

	
	public class ServerClient : MonoBehaviour {	

		private CancellationTokenSource tokenSource;
		public long responseCode { get; set; }	

		private void Awake() {
			tokenSource = new CancellationTokenSource();

		}

		public async Task<bool> HttpPostAsync(string domain, string page, string json) {
			CancellationToken token = tokenSource.Token;			
					string url = $"https://{domain}/{page}";
					using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, json)) {

						var operation = webRequest.SendWebRequest();
						while (!operation.isDone && !token.IsCancellationRequested) {
							await Task.Delay(100);
						}
						if (token.IsCancellationRequested) {
							webRequest.Abort();
							return false;
						}
						if (webRequest.result == UnityWebRequest.Result.Success) {

							return webRequest.responseCode == 200;
						}
						responseCode = webRequest.responseCode;
					}			
			
			
			return false;
		}


		public async Task<Texture2D> HttpGetTextureAsync(string domain) {
			Texture2D texture;
			CancellationToken token = tokenSource.Token;

			string url = domain;
			using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url)) {				
				var operation = webRequest.SendWebRequest();
				while (!operation.isDone && !token.IsCancellationRequested) {
					await Task.Delay(100);
				}
				if (token.IsCancellationRequested) {
					webRequest.Abort();
					return null;
				}
				if (webRequest.result == UnityWebRequest.Result.Success) {

					texture = DownloadHandlerTexture.GetContent(webRequest);
					return texture;

				}
			}
			return null;
		}
		public async Task<string> HttpGetAsync(string domain) {			
			CancellationToken token = tokenSource.Token;

			string url = domain;
			using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {				
				var operation = webRequest.SendWebRequest();
				while (!operation.isDone && !token.IsCancellationRequested) {
					await Task.Delay(100);
				}
				if (token.IsCancellationRequested) {
					webRequest.Abort();
					return null;
				}
				if (webRequest.result == UnityWebRequest.Result.Success) {

					
					return webRequest.downloadHandler.text;

				}
			}
			return null;
		}

		private void OnDestroy() {
			tokenSource?.Cancel();
		}
	}
}