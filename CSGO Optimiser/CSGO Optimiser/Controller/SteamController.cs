using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
