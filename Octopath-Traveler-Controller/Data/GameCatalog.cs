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
    {
        foreach (CharacterJson character in Characters)
            if (character.Name == name)
                return character;
        return null;
    }

    public EnemyJson? FindEnemy(string name)
    {
        foreach (EnemyJson enemy in Enemies)
            if (enemy.Name == name)
                return enemy;
        return null;
    }

    public bool HasSkill(string name)
    {
        foreach (SkillJson skill in Skills)
            if (skill.Name == name)
                return true;
        return false;
    }

    public bool HasPassiveSkill(string name)
    {
        foreach (PassiveSkillJson passiveSkill in PassiveSkills)
            if (passiveSkill.Name == name)
                return true;
        return false;
    }
}
