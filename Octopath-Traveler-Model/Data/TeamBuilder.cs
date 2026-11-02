using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Data.Skills;
using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

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
        Traveler traveler = BuildBaseTraveler(parsedTraveler, catalog);
        ApplyPassiveSkills(traveler, parsedTraveler.PassiveSkillNames);
        return traveler;
    }

    private static Traveler BuildBaseTraveler(ParsedTraveler parsedTraveler, GameCatalog catalog)
    {
        TravelerJson travelerData = RequireTraveler(parsedTraveler.Name, catalog);
        UnitStatsJson statsJson = RequireStats(travelerData.Stats, parsedTraveler.Name);
        return new Traveler(parsedTraveler.Name, BuildStats(statsJson),
            RequireSp(statsJson, parsedTraveler.Name), travelerData.Weapons,
            BuildActiveSkills(parsedTraveler.ActiveSkillNames, catalog));
    }

    private static BeastSkill BuildBeastSkill(string skillName, GameCatalog catalog)
        => BeastSkillFactory.Create(catalog.FindBeastSkill(skillName)
           ?? throw new InvalidDataException($"{skillName} no existe"));

    private static List<ActiveSkill> BuildActiveSkills(List<string> skillNames, GameCatalog catalog)
        => skillNames.Select(name => SkillFactory.Create(RequireSkill(name, catalog))).ToList();

    private static void ApplyPassiveSkills(Traveler traveler, List<string> passiveSkillNames)
    {
        foreach (PassiveSkill passiveSkill in PassiveSkillFactory.CreateAll(passiveSkillNames))
            passiveSkill.ApplyTo(traveler);
    }

    private static Beast BuildBeast(string beastName, GameCatalog catalog)
    {
        BeastJson beastData = RequireBeast(beastName, catalog);
        UnitStatsJson statsJson = RequireStats(beastData.Stats, beastName);
        BeastSkill skill = BuildBeastSkill(RequireSkillName(beastData.Skill, beastName), catalog);
        return new Beast(beastName, BuildStats(statsJson), skill, beastData.Shields, beastData.Weaknesses);
    }

    private static Stats BuildStats(UnitStatsJson statsJson)
        => new Stats(statsJson.HP, statsJson.PhysAtk, statsJson.PhysDef,
            statsJson.ElemAtk, statsJson.ElemDef, statsJson.Speed);

    private static TravelerJson RequireTraveler(string name, GameCatalog catalog)
        => catalog.FindTraveler(name)
           ?? throw new InvalidDataException($"{name} no existe");

    private static SkillJson RequireSkill(string name, GameCatalog catalog)
        => catalog.FindSkill(name)
           ?? throw new InvalidDataException($"{name} no existe");

    private static BeastJson RequireBeast(string name, GameCatalog catalog)
        => catalog.FindBeast(name)
           ?? throw new InvalidDataException($"{name} no existe");

    private static UnitStatsJson RequireStats(UnitStatsJson? statsJson, string unitName)
        => statsJson
           ?? throw new InvalidDataException($"{unitName} sin stats");

    private static int RequireSp(UnitStatsJson statsJson, string travelerName)
        => statsJson.SP
           ?? throw new InvalidDataException($"{travelerName} sin SP");

    private static string RequireSkillName(string? skillName, string beastName)
        => skillName
           ?? throw new InvalidDataException($"{beastName} sin habilidad");
}
