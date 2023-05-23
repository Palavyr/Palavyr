using System;
using System.IO;

namespace Palavyr.Core
{
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                // Console.WriteLine(line);
                var parts = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    // Console.WriteLine($"{parts[0]} + {parts[1]}");
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(string.Join("-- ERROR --", parts));
                }
                if (parts.Length != 2)
                    continue;
                
                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }

            ;
        }
    }
}