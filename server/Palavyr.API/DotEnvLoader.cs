using System;
using System.IO;

namespace Palavyr.API
{
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"{parts[0]} - {parts[1]}");
                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}