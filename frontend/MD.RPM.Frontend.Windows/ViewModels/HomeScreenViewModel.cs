using Caliburn.Micro;

namespace MD.RPM.Frontend.Windows.ViewModels;

public class HomeScreenViewModel : Screen
{
    public HomeScreenViewModel()
    {
        
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
        Console.WriteLine("Exiting...");
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