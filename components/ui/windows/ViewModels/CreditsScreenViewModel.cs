using Caliburn.Micro;

namespace MD.RPM.UI.Windows.ViewModels;

public class CreditsScreenViewModel : Screen
{
    private IScreenManager _screenManager;
    
    public CreditsScreenViewModel(IScreenManager screenManager)
    {
        _screenManager = screenManager;
    }
    
    public void Back()
    {
        _screenManager.ChangeScreen(AppScreen.HomeScreen);
    }
}