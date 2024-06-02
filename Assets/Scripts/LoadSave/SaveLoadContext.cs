using System.Threading.Tasks;

public class SaveLoadContext {
	private readonly ISaveLoadStrategy _strategy;

	public SaveLoadContext(ISaveLoadStrategy strategy) {
		_strategy = strategy;
	}

	public async Task SaveDataAsync(string data, string path) {
		await _strategy.Save(data, path);
	}

	public async Task<string> LoadDataAsync(string path) {
		return await _strategy.Load(path);
	}
}
