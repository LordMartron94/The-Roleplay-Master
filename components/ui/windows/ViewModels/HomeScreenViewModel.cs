using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;
using MD.RPM.UI.Communication;

namespace MD.RPM.UI.Windows.ViewModels;

public class HomeScreenViewModel : Screen
{
    private readonly SoftwareStateManager _softwareStateManager;
    private readonly IScreenManager _screenManager;
    private readonly API _api;
    
    public HomeScreenViewModel(IScreenManager screenManager)
    {
        _softwareStateManager = SoftwareStateManager.Instance;
        _api = API.Instance;

        _screenManager = screenManager ?? throw new ArgumentNullException(nameof(screenManager));
    }

    public void StartNewGame()
    {
        _screenManager.ChangeScreen(AppScreen.NewGameScreen);
        _api.CreateNewGame();
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