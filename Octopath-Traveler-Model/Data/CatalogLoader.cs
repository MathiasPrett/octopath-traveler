using System.Text.Json;
using Octopath_Traveler.Data.Json;

namespace Octopath_Traveler.Data;

public static class CatalogLoader
{
    private const string TravelersFile = "characters.json";
    private const string BeastsFile = "enemies.json";
    private const string SkillsFile = "skills.json";
    private const string PassiveSkillsFile = "passive_skills.json";
    private const string BeastSkillsFile = "beast_skills.json";

    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions { IncludeFields = true };

    public static GameCatalog LoadCatalog(string dataFolder)
        => new GameCatalog
        {
            Travelers = ReadJsonList<TravelerJson>(dataFolder, TravelersFile),
            Beasts = ReadJsonList<BeastJson>(dataFolder, BeastsFile),
            Skills = ReadJsonList<SkillJson>(dataFolder, SkillsFile),
            PassiveSkills = ReadJsonList<PassiveSkillJson>(dataFolder, PassiveSkillsFile),
            BeastSkills = ReadJsonList<BeastSkillJson>(dataFolder, BeastSkillsFile)
        };

    private static List<T> ReadJsonList<T>(string dataFolder, string fileName)
    {
        string path = Path.Combine(dataFolder, fileName);
        return Deserialize<T>(File.ReadAllText(path));
    }

    private static List<T> Deserialize<T>(string json)
        => JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
}
