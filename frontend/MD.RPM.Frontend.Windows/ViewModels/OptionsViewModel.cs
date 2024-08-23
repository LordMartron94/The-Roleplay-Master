using Caliburn.Micro;

namespace MD.RPM.Frontend.Windows.ViewModels;

public class OptionsViewModel : Screen
{
    private readonly IScreenManager _screenManager;

    public OptionsViewModel(IScreenManager screenManager)
    {
        _screenManager = screenManager;
    }
    
    public void Back()
    {
        _screenManager.ChangeScreen(AppScreen.HomeScreen);
    }
}