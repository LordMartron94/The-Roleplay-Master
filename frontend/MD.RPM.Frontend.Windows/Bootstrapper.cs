﻿using System.Windows;
using Caliburn.Micro;

namespace MD.RPM.Frontend.Windows.ViewModels;

public class Bootstrapper : BootstrapperBase
{
    private readonly SimpleContainer _container;
    
    public Bootstrapper()
    {
        _container = new SimpleContainer();
        
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