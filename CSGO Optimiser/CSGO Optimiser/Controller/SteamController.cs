using Model;
using System;

namespace Controller
{
    public static class SteamController
    {
        // Set the steam path
        public static void SetSteamPath(string selectedPath)
        {
            SteamPaths.Steam = selectedPath;
        }

        // Return steam path
        public static string GetSteamPath()
        {
            return SteamPaths.Steam;
        }

        public static void SetCsgoPath(string selectedPath)
        {
            SteamPaths.Csgo = selectedPath;
        }

        // Return csgo path (\Counter-Strike Global Offensive\)
        public static string GetCsgoPath()
        {
            return SteamPaths.Csgo;
        }

        // Check if steam path is different from null
        public static void ValidateSteamPath()
        {
            if (SteamPaths.Steam == null)
            {
                throw new Exception("Please locate your Steam folder.");
            }
        }

        public static void ValidateCsgoPath()
        {
            if (SteamPaths.Csgo == null)
            {
                throw new Exception("Please locate your Counter-Strike Global Offensive folder.");
            }
        }
    }
}
