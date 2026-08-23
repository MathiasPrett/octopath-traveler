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
    {
        foreach (ParsedTraveler traveler in travelers)
            if (!IsTravelerValid(traveler, catalog))
                return false;
        return true;
    }

    private static bool IsTravelerValid(ParsedTraveler traveler, GameCatalog catalog)
    {
        if (catalog.FindCharacter(traveler.Name) == null) return false;
        if (!AreActiveSkillsValid(traveler.ActiveSkillNames, catalog)) return false;
        return ArePassiveSkillsValid(traveler.PassiveSkillNames, catalog);
    }

    private static bool AreActiveSkillsValid(List<string> skillNames, GameCatalog catalog)
    {
        if (skillNames.Count > MaxActiveSkills) return false;
        if (HasRepeatedNames(skillNames)) return false;
        return DoAllActiveSkillsExist(skillNames, catalog);
    }

    private static bool ArePassiveSkillsValid(List<string> skillNames, GameCatalog catalog)
    {
        if (skillNames.Count > MaxPassiveSkills) return false;
        if (HasRepeatedNames(skillNames)) return false;
        return DoAllPassiveSkillsExist(skillNames, catalog);
    }

    private static bool DoAllActiveSkillsExist(List<string> skillNames, GameCatalog catalog)
    {
        foreach (string skillName in skillNames)
            if (!catalog.HasSkill(skillName))
                return false;
        return true;
    }

    private static bool DoAllPassiveSkillsExist(List<string> skillNames, GameCatalog catalog)
    {
        foreach (string skillName in skillNames)
            if (!catalog.HasPassiveSkill(skillName))
                return false;
        return true;
    }

    private static bool DoAllBeastsExist(List<string> beastNames, GameCatalog catalog)
    {
        foreach (string beastName in beastNames)
            if (catalog.FindEnemy(beastName) == null)
                return false;
        return true;
    }

    private static List<string> GetTravelerNames(List<ParsedTraveler> travelers)
    {
        var names = new List<string>();
        foreach (ParsedTraveler traveler in travelers)
            names.Add(traveler.Name);
        return names;
    }

    private static bool HasRepeatedNames(List<string> names)
    {
        var uniqueNames = new HashSet<string>(names);
        return uniqueNames.Count != names.Count;
    }

    private static bool IsCountInRange(int count, int minimum, int maximum)
        => count >= minimum && count <= maximum;
}
