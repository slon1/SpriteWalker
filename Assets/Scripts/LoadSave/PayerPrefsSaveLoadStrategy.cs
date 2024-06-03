using System.IO;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Provides a strategy for storing and loading data locally using playerprefs.
/// </summary>
public class PayerPrefsSaveLoadStrategy : ISaveLoadStrategy {
	
	public async Task Save(string data, string path) {
		PlayerPrefs.SetString(Path.GetFileName(path),data);
	}

	public async Task<string> Load(string path) {
		return PlayerPrefs.GetString(Path.GetFileName(path));
	}
	
}
