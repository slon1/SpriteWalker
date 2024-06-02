using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utils {
	public class SaveSystem : MonoBehaviour {

		private SaveLoadContext saveLoadContext;
		void Awake() {
			ISaveLoadStrategy localStrategy = new LocalSaveLoadStrategy();
			ISaveLoadStrategy remoteStrategy = new RemoteSaveLoadStrategy();			
			saveLoadContext = new SaveLoadContext(localStrategy);
		}
		public async void SaveGame(string data) {
			string path = Application.persistentDataPath + "/savegame.txt"; // ”кажите нужный путь
			await saveLoadContext.SaveDataAsync(data, path);
		}

		public async void LoadGame() {
			string path = Application.persistentDataPath + "/savegame.txt"; // ”кажите нужный путь
			string data = await saveLoadContext.LoadDataAsync(path);

			// ќбработайте загруженные данные
		}


	}
}
