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
			string path = Application.persistentDataPath + "/savegame.txt";
			await saveLoadContext.SaveDataAsync(data, path);
		}

		public async void LoadGame() {
			string path = Application.persistentDataPath + "/savegame.txt"; 
			string data = await saveLoadContext.LoadDataAsync(path);

			
		}


	}
}
