using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class PassiveSkillFactory
{
    // passive_skills.json describe sus efectos en texto libre, así que los bonus de stat
    // se registran acá. Las pasivas de entregas siguientes todavía no tienen efecto.
    private static readonly Dictionary<string, (StatType Stat, int Amount)> StatBonuses = new()
    {
        ["Elemental Augmentation"] = (StatType.ElemAtk, 50),
        ["Summon Strength"] = (StatType.PhysAtk, 50),
        ["Hale and Hearty"] = (StatType.HpMax, 500),
        ["Fleefoot"] = (StatType.Speed, 50),
        ["Inner Strength"] = (StatType.SpMax, 50)
    };

    public static PassiveSkill? Create(string name)
        => StatBonuses.TryGetValue(name, out (StatType Stat, int Amount) bonus)
            ? new PassiveSkill(name, bonus.Stat, bonus.Amount)
            : null;
}
