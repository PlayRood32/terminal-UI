using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ManagerCommands.Systems;

public class TerminalSysytem
{
    private Process _process;
    private StreamWriter _inputWriter;

    public event Action<string> OutputReceived;

    public void StartTerminal()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash",
            Arguments = OperatingSystem.IsWindows() ? "" : "-i", 
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8 
        };

        _process = new Process { StartInfo = startInfo };

        _process.OutputDataReceived += (s, e) => {
            if (e.Data != null) OutputReceived?.Invoke(e.Data);
        };
      
        _process.ErrorDataReceived += (s, e) => {
            if (e.Data != null) OutputReceived?.Invoke(e.Data);
        };

        _process.Start();

        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _inputWriter = _process.StandardInput;
    }

    public void SendCommand(string command)
    {
        if (_process != null && !_process.HasExited)
        {
            _inputWriter.WriteLine(command);
        }
    }
    
    public void StopTerminal()
    {
        try
        {
            if (_process != null && !_process.HasExited)
            {
                _process.Kill(); 
                _process.Dispose(); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping terminal: {ex.Message}");
        }
    }
}
