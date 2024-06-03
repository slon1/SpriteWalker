using Server;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
/// <summary>
/// Class for loading configuration data from either remote server or local source.
/// </summary>
public class ConfigLoader : MonoBehaviour
{
    private ServerClient server;
    [Inject]
    private void Construct(ServerClient server) {
        this.server = server;
    }
    [SerializeField]
    private bool useRemoteConfig;
    [SerializeField]
    private string remoteUrl;
    [SerializeField]
    private GameConfig localConfig;
	
	private async Task<Sprite> LoadSprite(string path, int ppu) {
		var texture = await server.HttpGetTextureAsync(path);
		Rect rect = new Rect(0, 0, texture.width, texture.height);
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		return Sprite.Create(texture, rect, pivot, ppu);
	}

	public async Task<GameData> LoadConfig() {
		if (useRemoteConfig) {
			var json = await server.HttpGetAsync(remoteUrl);
			GameData gameData = JsonUtility.FromJson<GameData>(json);
			gameData.board = await LoadSprite(gameData.boardUrl, gameData.boardPPU);
			return gameData;
		}
		else {
			return localConfig.gameData;
		}
	}


}
