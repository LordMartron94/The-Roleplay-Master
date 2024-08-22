using Caliburn.Micro;

namespace MD.RPM.Frontend.Windows.ViewModels;

public class ShellViewModel : Conductor<object>
{
    private HomeScreenViewModel _homeScreenViewModel;
    
    public ShellViewModel(HomeScreenViewModel homeScreenViewModel)
    {
        _homeScreenViewModel = homeScreenViewModel;
        
        // ReSharper disable once VirtualMemberCallInConstructor
        ActivateItemAsync(homeScreenViewModel);
    }
}