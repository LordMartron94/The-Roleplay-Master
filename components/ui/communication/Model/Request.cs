using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MD.RPM.UI.Communication.Model;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public struct Request
{
    public string source { get; set; }
    public Action[] actions { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}