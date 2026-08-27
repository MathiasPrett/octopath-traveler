using System.Text.Json;
using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Utils;

namespace Octopath_Traveler.Data;

public static class UnitDataLoader
{
    private const string CharactersFile = "characters.json";
    private const string EnemiesFile = "enemies.json";
    private const string SkillsFile = "skills.json";
    private const string PassiveSkillsFile = "passive_skills.json";
    private const string MissingFileMessage = "No se encontró el archivo";

    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions { IncludeFields = true };

    public static GameCatalog LoadCatalog(string dataFolder)
        => new GameCatalog(
            ReadJsonList<CharacterJson>(dataFolder, CharactersFile),
            ReadJsonList<EnemyJson>(dataFolder, EnemiesFile),
            ReadJsonList<SkillJson>(dataFolder, SkillsFile),
            ReadJsonList<PassiveSkillJson>(dataFolder, PassiveSkillsFile));

    private static List<T> ReadJsonList<T>(string dataFolder, string fileName)
    {
        string path = Path.Combine(dataFolder, fileName);
        FileGuard.EnsureExists(path, MissingFileMessage);
        return Deserialize<T>(File.ReadAllText(path));
    }

    private static List<T> Deserialize<T>(string json)
        => JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
}
