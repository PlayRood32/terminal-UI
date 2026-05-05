using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ManagerCommands.Models;

public class Commend
{

    [JsonPropertyName("name")]
    public string name{ get; set; }

[JsonPropertyName("location")]
    public string location { get; set; }
    
    [JsonPropertyName("content")]
    public string content  { get; set; }

     
    [JsonPropertyName("description")]
    public string description  { get; set; }
    
    // רשימה של פקודות מופסקות על ידי \n
   
    [JsonPropertyName("isChange")]
    public bool isChange { get; set; }
    //פקודה משתנה זה עם ערך משתנה מסומל על ידי פקודה ?משתנה? פקודה
    //אפשר גם פקודה ?משתנה? ?משתנה? פקודה ?משתנה? פקודה
    //אפשר גם פקודה ?משתנה? ?משתנה? פקודה ?משתנה?
    

    
}
