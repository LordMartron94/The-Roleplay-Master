using MD.RPM.UI.Communication._Internal;
using MD.RPM.UI.Communication.Model;
using Newtonsoft.Json;

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
    
    private string FormatMessageWrongTest(string actionName, string data = "")
    {
        return $"{{\"soce\":\"Windows UI Component\", \"actions\":[{{\"ne\":\"{actionName}\", \"da\":\"{data}\"}}]}}";    
    }
    

    private ServerResponse SendMessage(string jsonData)
    {
        if (_connector == null)
            throw new InvalidOperationException("API not initialized. Call Initialize() before using.");
        
        string response = Task.Run(async () => await _connector.SendData(jsonData)).GetAwaiter().GetResult()!;
        return JsonConvert.DeserializeObject<ServerResponse>(response);
    }

    public ServerResponse ShutdownMiddleman()
    {
        string jsonData = FormatMessage("Shutdown");
        return SendMessage(jsonData);
    }

    public ServerResponse TestMessage(string message)
    {
        string jsonData = FormatMessageWrongTest("Test", message);
        return SendMessage(jsonData);
    }

    public ServerResponse CreateNewGame()
    {
        string jsonData = FormatMessage("CreateNewGame");
        return SendMessage(jsonData);
    }
}