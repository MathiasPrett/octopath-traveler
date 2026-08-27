using Octopath_Traveler.Data.Json;

namespace Octopath_Traveler.Data;

public class GameCatalog
{
    public List<CharacterJson> Characters;
    public List<EnemyJson> Enemies;
    public List<SkillJson> Skills;
    public List<PassiveSkillJson> PassiveSkills;

    public GameCatalog(List<CharacterJson> characters, List<EnemyJson> enemies,
        List<SkillJson> skills, List<PassiveSkillJson> passiveSkills)
    {
        Characters = characters;
        Enemies = enemies;
        Skills = skills;
        PassiveSkills = passiveSkills;
    }

    public CharacterJson? FindCharacter(string name)
        => Characters.FirstOrDefault(character => character.Name == name);

    public EnemyJson? FindEnemy(string name)
        => Enemies.FirstOrDefault(enemy => enemy.Name == name);

    public bool HasSkill(string name)
        => Skills.Any(skill => skill.Name == name);

    public bool HasPassiveSkill(string name)
        => PassiveSkills.Any(passiveSkill => passiveSkill.Name == name);
}
