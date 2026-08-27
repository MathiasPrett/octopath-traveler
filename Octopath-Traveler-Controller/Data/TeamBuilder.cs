using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models;

namespace Octopath_Traveler.Data;

public static class TeamBuilder
{
    public static ValidatedTeam Build(ParsedTeamFile parsedTeam, GameCatalog catalog)
    {
        List<Traveler> travelers = BuildTravelers(parsedTeam.Travelers, catalog);
        List<Beast> beasts = BuildBeasts(parsedTeam.BeastNames, catalog);
        return new ValidatedTeam(travelers, beasts);
    }

    private static List<Traveler> BuildTravelers(List<ParsedTraveler> parsedTravelers, GameCatalog catalog)
        => parsedTravelers.Select(parsed => BuildTraveler(parsed, catalog)).ToList();

    private static List<Beast> BuildBeasts(List<string> beastNames, GameCatalog catalog)
        => beastNames.Select(beastName => BuildBeast(beastName, catalog)).ToList();

    private static Traveler BuildTraveler(ParsedTraveler parsedTraveler, GameCatalog catalog)
    {
        CharacterJson character = RequireCharacter(parsedTraveler.Name, catalog);
        UnitStatsJson statsJson = RequireStats(character.Stats, parsedTraveler.Name);
        int spMax = RequireSp(statsJson, parsedTraveler.Name);
        return new Traveler(parsedTraveler.Name, BuildStats(statsJson), spMax,
            character.Weapons, parsedTraveler.ActiveSkillNames, parsedTraveler.PassiveSkillNames);
    }

    private static Beast BuildBeast(string beastName, GameCatalog catalog)
    {
        EnemyJson enemy = RequireEnemy(beastName, catalog);
        UnitStatsJson statsJson = RequireStats(enemy.Stats, beastName);
        string skill = RequireSkillName(enemy.Skill, beastName);
        return new Beast(beastName, BuildStats(statsJson), skill, enemy.Shields, enemy.Weaknesses);
    }

    private static Stats BuildStats(UnitStatsJson statsJson)
        => new Stats(statsJson.HP, statsJson.PhysAtk, statsJson.PhysDef,
            statsJson.ElemAtk, statsJson.ElemDef, statsJson.Speed);

    private static CharacterJson RequireCharacter(string name, GameCatalog catalog)
        => catalog.FindCharacter(name)
           ?? throw new InvalidDataException($"El viajero {name} no existe en characters.json");

    private static EnemyJson RequireEnemy(string name, GameCatalog catalog)
        => catalog.FindEnemy(name)
           ?? throw new InvalidDataException($"La bestia {name} no existe en enemies.json");

    private static UnitStatsJson RequireStats(UnitStatsJson? statsJson, string unitName)
        => statsJson
           ?? throw new InvalidDataException($"La unidad {unitName} no tiene stats en el archivo JSON");

    private static int RequireSp(UnitStatsJson statsJson, string travelerName)
        => statsJson.SP
           ?? throw new InvalidDataException($"El viajero {travelerName} no tiene SP en characters.json");

    private static string RequireSkillName(string? skillName, string beastName)
        => skillName
           ?? throw new InvalidDataException($"La bestia {beastName} no tiene habilidad en enemies.json");
}
