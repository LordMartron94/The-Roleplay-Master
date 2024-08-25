using System.Diagnostics.CodeAnalysis;

namespace MD.RPM.UI.Communication.Model;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public struct Action
{
    public string name { get; set; }
    public string data { get; set; }
}