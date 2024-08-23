using Caliburn.Micro;

namespace MD.RPM.UI.Windows;

public interface IScreenManager
{
    void Initialize(SimpleContainer container);
    
    Screen? GetCurrentScreen();
    
    void ChangeScreen(AppScreen screen);
    
    void SubscribeToScreenChange(Action<Screen> callback);
    
    void UnsubscribeFromScreenChange(Action<Screen> callback);
}