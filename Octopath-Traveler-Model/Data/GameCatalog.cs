using Octopath_Traveler.Data.Json;

namespace Octopath_Traveler.Data;

public class GameCatalog
{
    public required IReadOnlyList<TravelerJson> Travelers { get; init; }
    public required IReadOnlyList<BeastJson> Beasts { get; init; }
    public required IReadOnlyList<SkillJson> Skills { get; init; }
    public required IReadOnlyList<PassiveSkillJson> PassiveSkills { get; init; }
    public required IReadOnlyList<BeastSkillJson> BeastSkills { get; init; }

    public TravelerJson? FindTraveler(string name)
        => Travelers.FirstOrDefault(traveler => traveler.Name == name);

    public BeastJson? FindBeast(string name)
        => Beasts.FirstOrDefault(beast => beast.Name == name);

    public SkillJson? FindSkill(string name)
        => Skills.FirstOrDefault(skill => skill.Name == name);

    public PassiveSkillJson? FindPassiveSkill(string name)
        => PassiveSkills.FirstOrDefault(passiveSkill => passiveSkill.Name == name);

    public BeastSkillJson? FindBeastSkill(string name)
        => BeastSkills.FirstOrDefault(beastSkill => beastSkill.Name == name);

    public bool HasSkill(string name)
        => FindSkill(name) != null;

    public bool HasPassiveSkill(string name)
        => FindPassiveSkill(name) != null;
}
