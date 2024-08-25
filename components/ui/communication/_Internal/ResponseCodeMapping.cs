namespace MD.RPM.UI.Communication._Internal;

public static class ResponseCodeMapping
{
    public static Dictionary<int, ResponseCode> Mapping { get; } = new Dictionary<int, ResponseCode>
    {
        { 200, ResponseCode.Success },
        { 399, ResponseCode.InvalidAction },
        { 400, ResponseCode.InvalidRequestFormat },
        { -1, ResponseCode.UnknownError }
    };
}