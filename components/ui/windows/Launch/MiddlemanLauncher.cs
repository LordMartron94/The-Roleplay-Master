using System.Diagnostics;
using MD.RPM.UI.Communication;

namespace MD.RPM.UI.Windows.Launch;

/// <summary>
/// Responsible for launching the Middleman Router written in Go and closing it when the application is closed.
/// </summary>
/// Todo: Make it subscribe to the application shut down event with a weight of 100 to ensure it gets executed last. (thus also implement weight system there)
public class MiddlemanLauncher
{
    private readonly Process _process;
    private readonly API _api;

    public MiddlemanLauncher(API api)
    {
        _api = api;
        _process = new Process();
        _process.StartInfo.FileName = @"D:\10 Work\Programming\00 Current Projects\The Roleplay Master\components\communication\middleman.exe";
        _process.StartInfo.WorkingDirectory = @"D:\10 Work\Programming\00 Current Projects\The Roleplay Master\components\communication"; 
    }

    public void Launch()
    {
        _process.Start();
    }

    public void Close()
    {
        _api.ShutdownMiddleman();
        _process.WaitForExit();
    }
}