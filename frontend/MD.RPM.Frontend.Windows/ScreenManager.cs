using Caliburn.Micro;
using MD.RPM.Frontend.Windows.ViewModels;

namespace MD.RPM.Frontend.Windows;

public sealed class ScreenManager : IScreenManager
{
    private Dictionary<AppScreen, Screen?>? _screens;
    private Screen? _currentScreen;
    
    private List<Action<Screen>> _screenChangeSubscriptions;

    private bool _initialized;

    public ScreenManager()
    {
        _screenChangeSubscriptions = new List<Action<Screen>>();
        _initialized = false;
    }

    public void Initialize(SimpleContainer container)
    {
        if (_initialized)
            throw new InvalidOperationException("ScreenManager has already been initialized. Something is wrong if you try to initialize it twice.");
        
        _initialized = true;
        
        _screens = new Dictionary<AppScreen, Screen?>
        {
            { AppScreen.HomeScreen, container.GetInstance<HomeScreenViewModel>() },
            { AppScreen.CreditsScreen, container.GetInstance<CreditsScreenViewModel>() }
        };
    }

    public void SubscribeToScreenChange(Action<Screen> action)
    {
        if (!_screenChangeSubscriptions.Contains(action))
            _screenChangeSubscriptions.Add(action);
    }
    
    public void UnsubscribeFromScreenChange(Action<Screen> action)
    {
        _screenChangeSubscriptions.Remove(action);
    }

    public Screen? GetCurrentScreen()
    {
        return _currentScreen;
    }

    public async void ChangeScreen(AppScreen screen)
    {
        if (!_initialized)
            throw new InvalidOperationException("ScreenManager has not been initialized. Initialize it before calling ChangeScreen.");
        
        if (_currentScreen != null)
            await _currentScreen.DeactivateAsync(false);

        if (!_screens!.TryGetValue(screen, out Screen? screenInstance))
            throw new InvalidOperationException($"Screen '{screen}' not found.");

        _currentScreen = screenInstance;
        
        foreach (Action<Screen> action in _screenChangeSubscriptions)
            action.Invoke(screenInstance!);
    }
}