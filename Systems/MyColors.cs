
using System.Collections.Generic;
using Avalonia.Media;
using System.Linq;

namespace ManagerCommands.Systems;

public class MyColors
{
    
      public static List<string> AllFonts = FontManager.Current.SystemFonts
        .Select(f => f.Name)
        .OrderBy(name => name) 
        .ToList();
    
   public static List<string> colors = new List<string>
{
    "NONU", "White", "Blue", "Green", "Black", "Red", "Grey", "Yellow",
    "Orange", "Purple", "Pink", "Brown", "Gold", "Silver", "Cyan", "Magenta",
    "Lime", "Teal", "Navy", "Maroon", "Olive", "Indigo", "Violet", "SkyBlue"
};
   
   public static List<string> colorsForBackgroun = new List<string>
   {
       "default",  "Blue", "Green", "Black", "Red", "Grey", "Yellow",
       "Orange", "Purple", "Pink", "Brown", "Gold", "Silver", "Cyan", "Magenta",
       "Lime", "Teal", "Navy", "Maroon", "Olive", "Indigo", "Violet", "SkyBlue"
   };

public static IBrush GetColorByName(string name)
{
    try
    {
        if (string.IsNullOrEmpty(name))
        {
            return Brushes.Transparent;
        }
        
        if (name.Length == 6 && name.All(c => "0123456789ABCDEFabcdef".Contains(c)))
        {
            name = "#" + name;
        }

        if (!colors.Contains(name) || !AllFonts.Contains(name))
        {
            try
            {
                return (IBrush)Brush.Parse(name);
            }
            catch
            {
                return (IBrush)Brush.Parse("#14172D");
            }
        }
        switch (name)
        {
            
            case "Red":
                return Brushes.Red;
            case "Blue":
                return Brushes.Blue;
            case "Green":
                return Brushes.Green;
            case "Black":
                return Brushes.Black;
            case "White":
                return Brushes.White;
            case "Grey":
                return Brushes.Gray;
            case "Yellow":
                return Brushes.Yellow;
            case "Orange":
                return Brushes.Orange;
            case "Purple":
                return Brushes.Purple;
            case "Pink":
                return Brushes.Pink;
            case "Brown":
                return Brushes.Brown;
            case "Gold":
                return Brushes.Gold;
            case "Silver":
                return Brushes.Silver;
            case "Cyan":
                return Brushes.Cyan;
            case "Magenta":
                return Brushes.Magenta;
            case "Lime":
                return Brushes.Lime;
            case "Teal":
                return Brushes.Teal;
            case "Navy":
                return Brushes.Navy;
            case "Maroon":
                return Brushes.Maroon;
            case "Olive":
                return Brushes.Olive;
            case "Indigo":
                return Brushes.Indigo;
            case "Violet":
                return Brushes.Violet;
            case "SkyBlue":
                return Brushes.SkyBlue;
            case "NONU":
                return Brushes.Transparent;
            case "default":
                return (IBrush)Brush.Parse("#14172D");
            default:
                try
                {
                    return Brush.Parse(name);
                }
                catch
                {
                    return Brushes.Transparent;
                }
        }
    }
    catch
    {
        return Brushes.Transparent;
    }
}
    
    
    
}
