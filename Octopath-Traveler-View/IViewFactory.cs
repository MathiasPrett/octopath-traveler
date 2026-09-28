namespace Octopath_Traveler_View;

public interface IViewFactory
{
    ITeamView CreateTeamView();
    IRoundView CreateRoundView();
    IBattleView CreateBattleView();
}
