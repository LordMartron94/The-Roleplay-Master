using MD.RPM.UI.Communication._Internal;

namespace MD.RPM.UI.Communication;

/// <summary>
/// API interface for communication with the server.
/// </summary>
public class API
{
    private Connector? _connector;

    public void Initialize()
    {
        _connector = new Connector("127.0.0.1", 8080);
    }

    private string FormatMessage(string actionName, string data = "")
    {
        return $"{{\"source\":\"Windows UI Component\", \"actions\":[{{\"name\":\"{actionName}\", \"data\":\"{data}\"}}]}}";    
    }
    

    private void SendMessage(string jsonData)
    {
        if (_connector == null)
            throw new InvalidOperationException("API not initialized. Call Initialize() before using.");
        
        string? response = Task.Run(async () => await _connector.SendData(jsonData)).GetAwaiter().GetResult();
        
        Console.WriteLine($"Received response: {response}");
    }

    public void ShutdownMiddleman()
    {
        string jsonData = FormatMessage("Shutdown");
        SendMessage(jsonData);
    }

    public void TestMessage(string message)
    {
        string jsonData = FormatMessage("Test", message);
        SendMessage(jsonData);
    }

    public void CreateNewGame()
    {
        string jsonData = FormatMessage("CreateNewGame");
        SendMessage(jsonData);
    }
}