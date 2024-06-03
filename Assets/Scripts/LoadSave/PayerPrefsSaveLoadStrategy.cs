using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PayerPrefsSaveLoadStrategy : ISaveLoadStrategy {
	
	public async Task Save(string data, string path) {
		PlayerPrefs.SetString(Path.GetFileName(path),data);
	}

	public async Task<string> Load(string path) {
		return PlayerPrefs.GetString(Path.GetFileName(path));
	}
	
}
