namespace Octopath_Traveler_View;

public class OptionReader
{
    private readonly View _view;

    public OptionReader(View view)
    {
        _view = view;
    }

    public int Read()
        => int.Parse(_view.ReadLine());
}
