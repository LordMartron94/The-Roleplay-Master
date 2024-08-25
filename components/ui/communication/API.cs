using MD.RPM.UI.Communication._Internal;
using MD.RPM.UI.Communication.Model;
using Newtonsoft.Json;

namespace MD.RPM.UI.Communication;

/// <summary>
/// API interface for communication with the server.
/// </summary>
public sealed class API
{
    private static volatile API? _instance;
    private readonly static object SyncRoot = new object();
    
    private Connector? _connector;
    private readonly RequestFactory _requestFactory; 
    private readonly ResponseHandler _responseHandler;

    private API()
    {
        _requestFactory = new RequestFactory();
        _responseHandler = new ResponseHandler();
    }

    public static API Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            lock (SyncRoot)
            {
                // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
                if (_instance == null)
                    _instance = new API();
            }

            return _instance;
        }
    }
    

    public void Initialize()
    {
        _connector = new Connector("127.0.0.1", 8080);
    }
    

    private ServerResponse SendMessage(Request request)
    {
        if (_connector == null)
            throw new InvalidOperationException("API not initialized. Call Initialize() before using.");

        string jsonData = request.ToJson();
        
        string response = Task.Run(async () => await _connector.SendData(jsonData)).GetAwaiter().GetResult()!;
        ServerResponse serverResponse = JsonConvert.DeserializeObject<ServerResponse>(response);
        
        _responseHandler.HandleResponse(serverResponse);
        return serverResponse;
    }

    public ServerResponse ShutdownMiddleman()
    {
        Request request = _requestFactory.CreateRequest(new Dictionary<string, string> { { "Shutdown", "" } });
        return SendMessage(request);
    }

    public ServerResponse CreateNewGame()
    {
        Request request = _requestFactory.CreateRequest(new Dictionary<string, string> { { "CreateNewGame", "" } });
        return SendMessage(request);
    }
}