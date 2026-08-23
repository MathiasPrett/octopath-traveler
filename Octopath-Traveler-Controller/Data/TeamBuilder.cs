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
    {
        var travelers = new List<Traveler>();
        foreach (ParsedTraveler parsedTraveler in parsedTravelers)
            travelers.Add(BuildTraveler(parsedTraveler, catalog));
        return travelers;
    }

    private static List<Beast> BuildBeasts(List<string> beastNames, GameCatalog catalog)
    {
        var beasts = new List<Beast>();
        foreach (string beastName in beastNames)
            beasts.Add(BuildBeast(beastName, catalog));
        return beasts;
    }

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
    {
        CharacterJson? character = catalog.FindCharacter(name);
        if (character == null)
            throw new InvalidDataException($"El viajero {name} no existe en characters.json");
        return character;
    }

    private static EnemyJson RequireEnemy(string name, GameCatalog catalog)
    {
        EnemyJson? enemy = catalog.FindEnemy(name);
        if (enemy == null)
            throw new InvalidDataException($"La bestia {name} no existe en enemies.json");
        return enemy;
    }

    private static UnitStatsJson RequireStats(UnitStatsJson? statsJson, string unitName)
    {
        if (statsJson == null)
            throw new InvalidDataException($"La unidad {unitName} no tiene stats en el archivo JSON");
        return statsJson;
    }

    private static int RequireSp(UnitStatsJson statsJson, string travelerName)
    {
        if (statsJson.SP == null)
            throw new InvalidDataException($"El viajero {travelerName} no tiene SP en characters.json");
        return statsJson.SP.Value;
    }

    private static string RequireSkillName(string? skillName, string beastName)
    {
        if (skillName == null)
            throw new InvalidDataException($"La bestia {beastName} no tiene habilidad en enemies.json");
        return skillName;
    }
}
