using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class CombatRenderer
{
    private const string Separator = "----------------------------------------";
    private const string RoundStartHeader = "INICIA RONDA";
    private const string PlayerTeamHeader = "Equipo del jugador";
    private const string EnemyTeamHeader = "Equipo del enemigo";
    private const string CurrentRoundQueueHeader = "Turnos de la ronda";
    private const string NextRoundQueueHeader = "Turnos de la siguiente ronda";
    private const string TurnHeader = "Turno de";
    private const string WeaponMenuHeader = "Seleccione un arma";
    private const string SkillMenuHeader = "Seleccione una habilidad para";
    private const string TargetMenuHeader = "Seleccione un objetivo para";
    private const string BoostPointPrompt = "Seleccione cuantos BP utilizar";
    private const string CancelOption = "Cancelar";
    private const string FleeMessage = "El equipo de viajeros ha huido!";
    private const string PlayerVictoryMessage = "Gana equipo del jugador";
    private const string EnemyVictoryMessage = "Gana equipo del enemigo";
    private const string AttacksMessage = "ataca";
    private const string UsesMessage = "usa";
    private const string PhysicalDamage = "físico";
    private const string WeaponDamage = "de tipo";
    private const char FirstPositionLetter = 'A';
    private const string QueueSeparator = ".";
    private const string MenuSeparator = ": ";

    private static readonly List<string> ActionOptions =
        new() { "Ataque básico", "Usar habilidad", "Defender", "Huir" };

    private readonly View _view;

    public CombatRenderer(View view)
    {
        _view = view;
    }

    public void ShowRoundStart(int round)
        => ShowBlock($"{RoundStartHeader} {round}");

    public void ShowTeamsState(ValidatedTeam team)
    {
        ShowSeparator();
        ShowTeamState(PlayerTeamHeader, DescribeAll(team.Travelers));
        ShowTeamState(EnemyTeamHeader, DescribeAll(team.Beasts));
    }

    public void ShowTurnQueues(List<Unit> currentRound, List<Unit> nextRound)
    {
        ShowQueue(CurrentRoundQueueHeader, currentRound);
        ShowQueue(NextRoundQueueHeader, nextRound);
    }

    public void ShowActionMenu(Traveler traveler)
        => ShowMenu($"{TurnHeader} {traveler.Name}", ActionOptions);

    public void ShowWeaponMenu(Traveler traveler)
        => ShowMenu(WeaponMenuHeader, WithCancel(traveler.Weapons));

    public void ShowSkillMenu(Traveler traveler)
        => ShowMenu($"{SkillMenuHeader} {traveler.Name}", WithCancel(traveler.ActiveSkills));

    public void ShowTargetMenu(Traveler traveler, List<Beast> targets)
        => ShowMenu($"{TargetMenuHeader} {traveler.Name}", WithCancel(DescribeAll(targets)));

    public void ShowBoostPointPrompt()
        => ShowBlock(BoostPointPrompt);

    public void ShowTravelerAttack(AttackOutcome outcome, string weaponName)
    {
        ShowBlock($"{outcome.Attacker.Name} {AttacksMessage}");
        ShowDamage(outcome, $"{WeaponDamage} {weaponName}");
    }

    public void ShowBeastAttack(AttackOutcome outcome, string skillName)
    {
        ShowBlock($"{outcome.Attacker.Name} {UsesMessage} {skillName}");
        ShowDamage(outcome, PhysicalDamage);
    }

    public void ShowFlee()
        => ShowBlock(FleeMessage);

    public void ShowPlayerVictory()
        => ShowBlock(PlayerVictoryMessage);

    public void ShowEnemyVictory()
        => ShowBlock(EnemyVictoryMessage);

    private void ShowTeamState(string header, List<string> descriptions)
    {
        _view.WriteLine(header);
        for (int index = 0; index < descriptions.Count; index++)
            _view.WriteLine($"{PositionLetter(index)}-{descriptions[index]}");
    }

    private void ShowQueue(string header, List<Unit> units)
        => ShowNumberedList(header, NamesOf(units), QueueSeparator);

    private void ShowMenu(string header, List<string> options)
        => ShowNumberedList(header, options, MenuSeparator);

    private void ShowNumberedList(string header, List<string> items, string separator)
    {
        ShowBlock(header);
        for (int index = 0; index < items.Count; index++)
            _view.WriteLine($"{index + 1}{separator}{items[index]}");
    }

    private void ShowDamage(AttackOutcome outcome, string damageDescription)
    {
        _view.WriteLine($"{outcome.Target.Name} recibe {outcome.Damage} de daño {damageDescription}");
        _view.WriteLine($"{outcome.Target.Name} termina con HP:{outcome.Target.Stats.HpCurrent}");
    }

    private void ShowBlock(string header)
    {
        ShowSeparator();
        _view.WriteLine(header);
    }

    private void ShowSeparator()
        => _view.WriteLine(Separator);

    private static string Describe(Traveler traveler)
        => $"{traveler.Name} - HP:{traveler.Stats.HpCurrent}/{traveler.Stats.HpMax}"
           + $" SP:{traveler.SpCurrent}/{traveler.SpMax} BP:{traveler.Bp}";

    private static string Describe(Beast beast)
        => $"{beast.Name} - HP:{beast.Stats.HpCurrent}/{beast.Stats.HpMax} Shields:{beast.Shields}";

    private static List<string> DescribeAll(List<Traveler> travelers)
        => travelers.Select(Describe).ToList();

    private static List<string> DescribeAll(List<Beast> beasts)
        => beasts.Select(Describe).ToList();

    private static List<string> NamesOf(List<Unit> units)
        => units.Select(unit => unit.Name).ToList();

    private static List<string> WithCancel(List<string> options)
        => options.Append(CancelOption).ToList();

    private static char PositionLetter(int index)
        => (char)(FirstPositionLetter + index);
}
