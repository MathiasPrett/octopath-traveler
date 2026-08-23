using System.Text.Json;
using Octopath_Traveler.Data.Json;

namespace Octopath_Traveler.Data;

public static class UnitDataLoader
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions { IncludeFields = true };

    public static GameCatalog LoadCatalog(string dataFolder)
    {
        return new GameCatalog(
            LoadCharacters(dataFolder),
            LoadEnemies(dataFolder),
            LoadSkills(dataFolder),
            LoadPassiveSkills(dataFolder));
    }

    public static List<CharacterJson> LoadCharacters(string dataFolder)
    {
        string path = Path.Combine(dataFolder, "characters.json");
        return ReadJsonList<CharacterJson>(path);
    }

    public static List<EnemyJson> LoadEnemies(string dataFolder)
    {
        string path = Path.Combine(dataFolder, "enemies.json");
        return ReadJsonList<EnemyJson>(path);
    }

    public static List<SkillJson> LoadSkills(string dataFolder)
    {
        string path = Path.Combine(dataFolder, "skills.json");
        return ReadJsonList<SkillJson>(path);
    }

    public static List<PassiveSkillJson> LoadPassiveSkills(string dataFolder)
    {
        string path = Path.Combine(dataFolder, "passive_skills.json");
        return ReadJsonList<PassiveSkillJson>(path);
    }

    private static List<T> ReadJsonList<T>(string path)
    {
        EnsureFileExists(path);
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
    }

    private static void EnsureFileExists(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"No se encontró el archivo: {path}");
    }
}
