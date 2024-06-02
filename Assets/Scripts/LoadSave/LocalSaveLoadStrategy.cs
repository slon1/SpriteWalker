using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LocalSaveLoadStrategy : ISaveLoadStrategy {
	public async Task Save(string data, string path) {
		await SaveToFileAsync(data, path);
	}

	public async Task<string> Load(string path) {
		return await LoadFromFileAsync(path);
	}

	private async Task SaveToFileAsync(string data, string path) {
		// Создаем необходимые директории, если их нет
		string directory = Path.GetDirectoryName(path);
		if (!Directory.Exists(directory)) {
			Directory.CreateDirectory(directory);
		}

		// Асинхронно записываем данные в файл
		using (StreamWriter writer = new StreamWriter(path, false)) {
			await writer.WriteAsync(data);
		}
	}

	private async Task<string> LoadFromFileAsync(string path) {
		if (!File.Exists(path)) {
			Debug.LogError("File not found: " + path);
			return null;
		}

		// Асинхронно читаем данные из файла
		using (StreamReader reader = new StreamReader(path)) {
			return await reader.ReadToEndAsync();
		}
	}
}
