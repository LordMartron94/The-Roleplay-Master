using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;

namespace MD.RPM.Frontend.Windows.ViewModels;

public class HomeScreenViewModel : Screen
{
    private readonly SoftwareStateManager _softwareStateManager;
    
    public HomeScreenViewModel()
    {
        _softwareStateManager = SoftwareStateManager.Instance;
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
        Console.WriteLine("Credits...");
    }
    
    public void Options()
    {
        Console.WriteLine("Options...");
    }
}