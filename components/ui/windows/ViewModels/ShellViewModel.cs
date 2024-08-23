using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;

namespace MD.RPM.UI.Windows.ViewModels;

public class ShellViewModel : Conductor<object>
{
    public ShellViewModel(IScreenManager screenManager)
    {
        screenManager.SubscribeToScreenChange(ChangeActiveItem);
        
        SoftwareStateManager softwareStateManager = SoftwareStateManager.Instance;
        
        softwareStateManager.SetExitHandler(() =>
        {
            TryCloseAsync();
        });
    }

    private void ChangeActiveItem(Screen screen)
    {
        ActiveItem = screen;
    }
}