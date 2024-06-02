using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Server {

	/// <summary>
	/// The server sends reconnectCount requests at intervals of reconnectTime seconds, 
	/// if the response is !200 and !403 then it switches to the backup server.
	/// </summary>
	public class ServerClient : MonoBehaviour {
		[SerializeField]
		private int reconnectCount = 3;
		[SerializeField]
		private int reconnectTime = 3;

		private CancellationTokenSource tokenSource;
		public long responseCode { get; set; }

		private string cookie;


		public string Cookie => cookie;
		private string fallBack;


		private void Awake() {
			tokenSource = new CancellationTokenSource();
			
		}



		private async void Start() {
			string rr = await HttpGetAsync("https://drive.google.com/uc?export=download&id=1GyrEyr6HNW7FZg0EsCO0FWwYtEH32IkL");
		
		}

		public async Task<bool> HttpPostAsync(string domain, string page, string json) {
			CancellationToken token = tokenSource.Token;
			string[] domainsToCheck = { domain, fallBack };
			for (int i = 0; i < reconnectCount; i++) {
				foreach (string domainToCheck in domainsToCheck) {
					string url = $"https://{domainToCheck}/{page}";
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
					await Task.Delay(reconnectTime);
				}
			}
			return false;
		}
		public Texture2D texture;

		public async Task<string> HttpGetAsync(string domain) {
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

					print(webRequest.downloadHandler.text);
					texture = DownloadHandlerTexture.GetContent(webRequest);
					return webRequest.responseCode == 200 ? webRequest.downloadHandler.text : null;

				}



			}
			return null;
		}

	

		private void OnDestroy() {
			tokenSource?.Cancel();
		}
	}
}