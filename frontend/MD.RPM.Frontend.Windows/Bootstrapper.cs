using System.Windows;
using Caliburn.Micro;
using MD.Common.SoftwareStateHandling;
using MD.RPM.Frontend.Windows.ViewModels;

namespace MD.RPM.Frontend.Windows;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer _container;
    
    public Bootstrapper()
    {
        _container = new SimpleContainer();
        StateDebugger _ = new StateDebugger();
        
        Initialize();
    }

    protected override void Configure()
    {
        _container.Instance(_container);
        
        _container
            .Singleton<IWindowManager, WindowManager>()
            .Singleton<IEventAggregator, EventAggregator>();
        
        GetType().Assembly.GetTypes()
            .Where(type => type.IsClass)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .ToList()
            .ForEach(viewModelType => _container.RegisterPerRequest(
                viewModelType, viewModelType.ToString(), viewModelType));
    }
    
    protected override void OnStartup(object sender, StartupEventArgs e)
    { 
        DisplayRootViewForAsync<ShellViewModel>();
    }
    
    protected override void OnExit(object sender, EventArgs e)
    {
        SoftwareStateManager softwareStateManager = SoftwareStateManager.Instance;
        
        softwareStateManager.Shutdown(true);
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