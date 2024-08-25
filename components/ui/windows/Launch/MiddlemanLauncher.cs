using System.Diagnostics;
using MD.RPM.UI.Communication;
using MD.RPM.UI.Communication.Model;

namespace MD.RPM.UI.Windows.Launch;

/// <summary>
/// Responsible for launching the Middleman Router written in Go and closing it when the application is closed.
/// </summary>
/// Todo: Make it subscribe to the application shut down event with a weight of 100 to ensure it gets executed last. (thus also implement weight system there)
public class MiddlemanLauncher
{
    private readonly Process _process;
    private readonly API _api;

    public MiddlemanLauncher(API api, bool visibleLauncherWindow = false)
    {
        _api = api;
        _process = new Process();
        _process.StartInfo.FileName = @"D:\10 Work\Programming\00 Current Projects\The Roleplay Master\components\communication\middleman.exe";
        _process.StartInfo.WorkingDirectory = @"D:\10 Work\Programming\00 Current Projects\The Roleplay Master\components\communication"; 
        
        // ReSharper disable once InvertIf
        if (!visibleLauncherWindow)
        {
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
        } 
    }

    public void Launch()
    {
        _process.Start();
    }

    public void Close()
    {
        ServerResponse response = _api.ShutdownMiddleman();
        
        if (response.code == 200)
            Console.WriteLine($"Middleman closed successfully with status code {response.code}");
        else
        {
            Console.WriteLine($"Failed to close Middleman. Status code: {response.code}");
            Console.WriteLine(response.message);
            Console.WriteLine("Will force kill the process.\nWarning, this can result in data loss.");
            _process.Kill();
        }
        
        _process.WaitForExit();
    }
}