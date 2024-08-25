using System.Windows;
using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;
using MD.RPM.UI.Communication;
using MD.RPM.UI.Communication.Model;
using MD.RPM.UI.Windows.Launch;
using MD.RPM.UI.Windows.ViewModels;

namespace MD.RPM.UI.Windows;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer _container;
    private readonly API _api;
    private readonly MiddlemanLauncher _middlemanLauncher;
    
    public Bootstrapper()
    {
        _container = new SimpleContainer();
        StateDebugger _ = new StateDebugger();
        _api = new API();
        _middlemanLauncher = new MiddlemanLauncher(_api, true);
        
        Initialize();
    }

    protected override void Configure()
    {
        _container.Instance(_container);

        _container
            .Singleton<IWindowManager, WindowManager>()
            .Singleton<IEventAggregator, EventAggregator>()
            .Singleton<IScreenManager, ScreenManager>();
        
        GetType().Assembly.GetTypes()
            .Where(type => type.IsClass)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .ToList()
            .ForEach(viewModelType => _container.RegisterPerRequest(
                viewModelType, viewModelType.ToString(), viewModelType));
        
        _container.GetInstance<IScreenManager>().Initialize(_container);
    }
    
    protected override void OnStartup(object sender, StartupEventArgs e)
    { 
        _middlemanLauncher.Launch();
        _api.Initialize();
        
        DisplayRootViewForAsync<ShellViewModel>();
        IScreenManager? screenManager = _container.GetInstance<IScreenManager>();
        screenManager.ChangeScreen(AppScreen.HomeScreen);
        
        // Tests
        ServerResponse response = _api.TestMessage("This is a test message sent from the Bootstrapper.");
        ServerResponse response2 = _api.CreateNewGame();
        
        Console.WriteLine($"Test message response status code: {response.code}");
        Console.WriteLine($"Create new game response status code: {response2.code}");
    }
    
    protected override void OnExit(object sender, EventArgs e)
    {
        SoftwareStateManager softwareStateManager = SoftwareStateManager.Instance;
        
        softwareStateManager.Shutdown(true);
        _middlemanLauncher.Close();
    }
    
    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }
    
    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }
    
    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }
}