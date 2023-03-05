using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample;
public class EntryPoint
{
    private static string logPath = @"C:\log.txt";
    public static void Log(object obj)
    {
        File.AppendAllText(logPath, $"{obj}\n");
    }

    public void Load()
    {
        File.WriteAllText(logPath, "");
        Log($"Injected at {DateTime.Now}");


    }

    public void Unload()
    {
        Log($"Uninjected at {DateTime.Now}");
    }
}