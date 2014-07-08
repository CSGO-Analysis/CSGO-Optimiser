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

        // Check if steam path is different from null
        public static void ValidateSteamPath()
        {
            if (SteamPaths.Steam == null)
            {
                throw new Exception("Please locate your Steam folder.");
            }
        }
    }
}
