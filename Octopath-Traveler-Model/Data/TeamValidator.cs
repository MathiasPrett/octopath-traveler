namespace Octopath_Traveler.Data;

public static class TeamValidator
{
    private const int MinTravelers = 1;
    private const int MaxTravelers = 4;
    private const int MinBeasts = 1;
    private const int MaxBeasts = 5;
    private const int MaxActiveSkills = 8;
    private const int MaxPassiveSkills = 4;

    public static bool IsValid(ParsedTeamFile team, GameCatalog catalog)
        => IsPlayerTeamValid(team.Travelers, catalog) && IsEnemyTeamValid(team.BeastNames, catalog);

    private static bool IsPlayerTeamValid(List<ParsedTraveler> travelers, GameCatalog catalog)
    {
        if (!IsCountInRange(travelers.Count, MinTravelers, MaxTravelers)) return false;
        if (HasRepeatedNames(GetTravelerNames(travelers))) return false;
        return AreAllTravelersValid(travelers, catalog);
    }

    private static bool IsEnemyTeamValid(List<string> beastNames, GameCatalog catalog)
    {
        if (!IsCountInRange(beastNames.Count, MinBeasts, MaxBeasts)) return false;
        if (HasRepeatedNames(beastNames)) return false;
        return DoAllBeastsExist(beastNames, catalog);
    }

    private static bool AreAllTravelersValid(List<ParsedTraveler> travelers, GameCatalog catalog)
        => travelers.All(traveler => IsTravelerValid(traveler, catalog));

    private static bool IsTravelerValid(ParsedTraveler traveler, GameCatalog catalog)
    {
        if (catalog.FindCharacter(traveler.Name) == null) return false;
        if (!AreActiveSkillsValid(traveler.ActiveSkillNames, catalog)) return false;
        return ArePassiveSkillsValid(traveler.PassiveSkillNames, catalog);
    }

    private static bool AreActiveSkillsValid(List<string> skillNames, GameCatalog catalog)
        => AreSkillsValid(skillNames, MaxActiveSkills, catalog.HasSkill);

    private static bool ArePassiveSkillsValid(List<string> skillNames, GameCatalog catalog)
        => AreSkillsValid(skillNames, MaxPassiveSkills, catalog.HasPassiveSkill);

    private static bool AreSkillsValid(List<string> skillNames, int maxSkills, Func<string, bool> exists)
    {
        if (skillNames.Count > maxSkills) return false;
        if (HasRepeatedNames(skillNames)) return false;
        return skillNames.All(exists);
    }

    private static bool DoAllBeastsExist(List<string> beastNames, GameCatalog catalog)
        => beastNames.All(beastName => catalog.FindEnemy(beastName) != null);

    private static List<string> GetTravelerNames(List<ParsedTraveler> travelers)
        => travelers.Select(traveler => traveler.Name).ToList();

    private static bool HasRepeatedNames(List<string> names)
    {
        var uniqueNames = new HashSet<string>(names);
        return uniqueNames.Count != names.Count;
    }

    private static bool IsCountInRange(int count, int minimum, int maximum)
        => count >= minimum && count <= maximum;
}
