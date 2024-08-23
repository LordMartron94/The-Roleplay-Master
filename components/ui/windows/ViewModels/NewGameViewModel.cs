using Caliburn.Micro;

namespace MD.RPM.UI.Windows.ViewModels;

public class NewGameViewModel : Screen
{
    private readonly IScreenManager _screenManager;

    public NewGameViewModel(IScreenManager screenManager)
    {
        _screenManager = screenManager;
    }
    
    public void Back()
    {
        _screenManager.ChangeScreen(AppScreen.HomeScreen);
    }
}