using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;

namespace MD.RPM.Frontend.Windows.ViewModels;

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
        Console.WriteLine("Starting new game...");
    }
    
    public void LoadGame()
    {
        Console.WriteLine("Loading game...");
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
        Console.WriteLine("Options...");
    }
}