using System;
using System.IO;

namespace MachinaWrapper.Common
{
    static class Util
    {
        /// <summary>
        /// Camelcases a string.
        /// </summary>
        public static void JSify(ref string input)
        {
            input = input.Substring(0, 1).ToLower() + input.Substring(1);
        }

        /// <summary>
        /// Return the Program Files (x86) location.
        /// See https://stackoverflow.com/a/194223
        /// </summary>
        public static string ProgramFilesx86()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? @"C:\Program Files (x86)";
            }

            return Environment.GetEnvironmentVariable("ProgramFiles") ?? @"C:\Program Files";
        }

        /// <summary>
        /// Returns true if a Korean client can be found.
        /// </summary>
        public static bool SystemHasKRClient()
        {
            string[] folders = {
                @"SquareEnix\FINAL FANTASY XIV - KOREA",
                @"FINAL FANTASY XIV - KOREA"
            };

            foreach (string folder in folders)
            {
                if (Directory.Exists(Path.Combine(ProgramFilesx86(), folder)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
