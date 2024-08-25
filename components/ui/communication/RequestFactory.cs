using MD.RPM.UI.Communication.Model;
using Action = MD.RPM.UI.Communication.Model.Action;

namespace MD.RPM.UI.Communication;

public class RequestFactory
{
    public Request CreateRequest(Action[] actions)
    {
        return new Request
        {
            source = "Windows UI Component",
            actions = actions
        };
    }

    public Request CreateRequest(Dictionary<string, string> actions)
    {
        Action[] actionArray = actions.Select(kvp => new Action { name = kvp.Key, data = kvp.Value }).ToArray();
        
        return CreateRequest(actionArray);
    }
}