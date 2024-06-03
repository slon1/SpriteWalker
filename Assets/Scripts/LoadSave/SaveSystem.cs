using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Utils {
	/// <summary>
	/// Manages the saving and loading of game data using various save/load strategies.
	/// </summary>
	public class SaveSystem : MonoBehaviour {

		private SaveLoadContext saveLoadContext;
		void Awake() {
			ISaveLoadStrategy localStrategy = new LocalSaveLoadStrategy();
			ISaveLoadStrategy remoteStrategy = new RemoteSaveLoadStrategy();
			ISaveLoadStrategy playerPrefsStrategy = new PayerPrefsSaveLoadStrategy();
			saveLoadContext = new SaveLoadContext(playerPrefsStrategy);
		}
		public async void SaveGame(string data) {
			string path = Application.persistentDataPath + "/savegame.txt";
			await saveLoadContext.SaveDataAsync(data, path);
		}

		public async Task<string> LoadGame() {
			string path = Application.persistentDataPath + "/savegame.txt";
			return await saveLoadContext.LoadDataAsync(path);

		}


	}
}
