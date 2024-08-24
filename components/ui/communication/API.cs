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

    private void SendMessage(string jsonData)
    {
        if (_connector == null)
            throw new InvalidOperationException("API not initialized. Call Initialize() before using.");
        
        string? response = Task.Run(async () => await _connector.SendData(jsonData)).GetAwaiter().GetResult();
        
        Console.WriteLine($"Received response: {response}");
    }

    public void ShutdownMiddleman()
    {
        const string jsonData = "{\"action\": \"shutdown\"}";
        SendMessage(jsonData);
    }

    public void TestMessage(string message)
    {
        const string jsonData = "{{ \"type\": \"test\", \"message\": \"{message}\" }}";
        SendMessage(jsonData);
    }

    public void CreateNewGame()
    {
        const string jsonData = "{ \"actions\": \"create_game\" }";
        SendMessage(jsonData);
    }
}