﻿using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;

namespace MD.RPM.Frontend.Windows.ViewModels;

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
        Console.WriteLine($"Active screen changed to {screen.GetType().Name}");
        ActivateItemAsync(screen);
    }
}