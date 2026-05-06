using System.Text.Json;
using ManagerCommands.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ManagerCommands.Systems;

public class DataSystem
{
    
    
    private readonly string _basePath;
    private readonly string _usersPath;
    private string UserFilePath => Path.Combine(_usersPath, "current_user.json");
    public DataSystem()
    {
        _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyCommend");
        _usersPath = Path.Combine(_basePath, "Users");
        Directory.CreateDirectory(_usersPath);
    }
    
    
    public async Task SaveUser(User user)
    {
        string json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(UserFilePath, json);
    }
    
    public async Task<User?> LoadUser()
    {
        if (!File.Exists(UserFilePath)) return null;

        string json = await File.ReadAllTextAsync(UserFilePath);
        return JsonSerializer.Deserialize<User>(json);
    }

    public string GetUserFilePath()
    {
        return UserFilePath;
    }
    
    
}
