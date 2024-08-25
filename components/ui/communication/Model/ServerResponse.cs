using System.Diagnostics.CodeAnalysis;

namespace MD.RPM.UI.Communication.Model;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public struct ServerResponse
{
    public string message { get; set; }
    public int code { get; set; }
}