using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class PassiveSkillFactory
{
    private static readonly Dictionary<string, (StatType Stat, int Amount)> StatBonuses = new()
    {
        ["Elemental Augmentation"] = (StatType.ElemAtk, 50),
        ["Summon Strength"] = (StatType.PhysAtk, 50),
        ["Hale and Hearty"] = (StatType.MaxHp, 500),
        ["Fleefoot"] = (StatType.Speed, 50),
        ["Inner Strength"] = (StatType.MaxSp, 50)
    };

    public static List<PassiveSkill> CreateAll(List<string> names)
        => names.Where(StatBonuses.ContainsKey).Select(Create).ToList();

    private static PassiveSkill Create(string name)
    {
        (StatType stat, int amount) = StatBonuses[name];
        return new PassiveSkill(stat, amount);
    }
}
