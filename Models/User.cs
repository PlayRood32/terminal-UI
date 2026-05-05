using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace ManagerCommands.Models;

public class User
{
    [JsonPropertyName("Commends")]
    public List<Commend> Commends{ get; set; }

    [JsonPropertyName("TimeWait")] 
    public int TimeWait { get; set; } = 400;

    [JsonPropertyName("sizeFont")]
    public int sizeFont { get; set; }
    
    [JsonPropertyName("fontFamily")]
    public string fontFamily { get; set; }
    [JsonPropertyName("ColorText")]
    public string ColorText { get; set; }
    [JsonPropertyName("ColorTerminal")]
    public string ColorTerminal { get; set; }

    public bool NotNull()
    {
        return sizeFont != null && sizeFont > 0 && ColorText != null && ColorTerminal != null && fontFamily != null;
    }


}
