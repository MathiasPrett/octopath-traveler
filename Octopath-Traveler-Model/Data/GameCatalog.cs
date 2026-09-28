using Octopath_Traveler.Data.Json;

namespace Octopath_Traveler.Data;

public class GameCatalog
{
    public required IReadOnlyList<CharacterJson> Characters { get; init; }
    public required IReadOnlyList<EnemyJson> Enemies { get; init; }
    public required IReadOnlyList<SkillJson> Skills { get; init; }
    public required IReadOnlyList<PassiveSkillJson> PassiveSkills { get; init; }
    public required IReadOnlyList<BeastSkillJson> BeastSkills { get; init; }

    public CharacterJson? FindCharacter(string name)
        => Characters.FirstOrDefault(character => character.Name == name);

    public EnemyJson? FindEnemy(string name)
        => Enemies.FirstOrDefault(enemy => enemy.Name == name);

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
