using System;
using System.IO;

namespace Identity.Utils
{
    public static class Env
    {
        public static void LoadVariables(string filePath = "../.env")
        {
            if (!File.Exists(filePath)) throw new DirectoryNotFoundException(filePath);

            foreach (var line in File.ReadAllLines(filePath))
            {
                var lineParts = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
                if (lineParts.Length != 2) continue;
                
                Environment.SetEnvironmentVariable(lineParts[0], lineParts[1]);
            }
        }
    }
}