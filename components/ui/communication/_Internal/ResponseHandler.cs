using MD.RPM.UI.Communication.Model;

namespace MD.RPM.UI.Communication._Internal;

/// <summary>
/// Handles incoming responses from the server.
/// </summary>
public class ResponseHandler
{
    public void HandleResponse(ServerResponse response)
    {
        ResponseCode code = ResponseCodeMapping.Mapping[response.code];

        switch (code)
        {
            case ResponseCode.Success:
                Console.WriteLine("Successful response received.");
                break;
            case ResponseCode.InvalidAction:
                Console.WriteLine("Invalid action sent: " + response.message);
                break;
            case ResponseCode.InvalidRequestFormat:
                Console.WriteLine("Invalid request format: " + response.message);
                break;
            case ResponseCode.UnknownError:
            default:
                Console.WriteLine("Unknown error has occurred: " + response.code);
                return;
        }
    }
}