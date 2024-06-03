using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class PayerPrefsSaveLoadStrategy : ISaveLoadStrategy {
	private string id = Application.productName.Trim();
	public async Task Save(string data, string path) {
		PlayerPrefs.SetString(id,data);
	}

	public async Task<string> Load(string path) {
		return PlayerPrefs.GetString(id);
	}
	
}
