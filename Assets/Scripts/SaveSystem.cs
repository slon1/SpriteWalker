using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Utils {
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
