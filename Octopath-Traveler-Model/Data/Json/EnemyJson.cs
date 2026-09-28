namespace Octopath_Traveler.Data.Json;

public class EnemyJson
{
    public string? Name;
    public UnitStatsJson? Stats;
    public string? Skill;
    public int Shields;
    public List<string> Weaknesses = new List<string>();
}
