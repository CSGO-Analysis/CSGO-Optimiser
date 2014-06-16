using Model;
using System;

namespace Controller
{
    public static class SteamController
    {
        public static void SetSteamPath(string selectedPath)
        {
            SteamPaths.Steam = selectedPath;
        }

        public static string GetSteamPath()
        {
            return SteamPaths.Steam;
        }

        public static void ValidateSteamPath()
        {
            if (SteamPaths.Steam == null)
            {
                throw new Exception("Please locate your Steam folder.");
            }
        }
    }
}
