using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;

namespace MD.RPM.UI.Windows.ViewModels;

public class HomeScreenViewModel : Screen
{
    private readonly SoftwareStateManager _softwareStateManager;
    private readonly IScreenManager _screenManager;
    
    public HomeScreenViewModel(IScreenManager screenManager)
    {
        _softwareStateManager = SoftwareStateManager.Instance;

        _screenManager = screenManager ?? throw new ArgumentNullException(nameof(screenManager));
    }

    public void StartNewGame()
    {
        _screenManager.ChangeScreen(AppScreen.NewGameScreen);
    }
    
    public void LoadGame()
    {
        _screenManager.ChangeScreen(AppScreen.LoadGameScreen);
    }
    
    public void Exit()
    {
        _softwareStateManager.Shutdown();
    }
    
    public void Credits()
    {
        _screenManager.ChangeScreen(AppScreen.CreditsScreen);
    }
    
    public void Options()
    {
        _screenManager.ChangeScreen(AppScreen.OptionsScreen);
    }
}