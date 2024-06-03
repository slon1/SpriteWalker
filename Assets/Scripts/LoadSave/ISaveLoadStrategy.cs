using System.Threading.Tasks;

public interface ISaveLoadStrategy {
	Task Save(string data, string path);
	Task<string> Load(string path);
}
public interface IConfigLoader {
	Task<GameData> LoadConfig();
}