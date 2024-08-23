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

    public void ShutdownMiddleman()
    {
        _connector?.SendData("{\"action\": \"shutdown\"}");
    }

    public void TestMessage(string message)
    {
        _connector?.SendData($"{{ \"type\": \"test\", \"message\": \"{message}\" }}");
    }

    public void CreateNewGame()
    {
        _connector?.SendData("{ \"actions\": \"create_game\" }");
    }
}