using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class ConditionFactory
{
    // Toda habilidad del catálogo actual se puede usar sin restricciones extra.
    // Las condiciones especiales de entregas futuras se registran acá y en ningún otro lugar.
    public static Condition Create(SkillJson skill) => new TrueCond();
}
