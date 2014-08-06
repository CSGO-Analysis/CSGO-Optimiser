using System;
using System.IO;

namespace Model
{
    public static class SteamPaths
    {
        private static string _steam;
        private static string _csgo;

        public static string Steam
        {
            get { return _steam; }
            set
            {
                validateSteamPath(value);
                checkIfCsgoIsLocatedWithin(value);
                _steam = value;
            }
        }

        public static string Csgo // This is the "\Counter-Strike Global Offensive\" folder
        {
            get { return _csgo;  }
            set
            {
                validateCsgoPath(value);
                _csgo = value;
                CsgoExe = value + @"\csgo.exe";
                CfgFolder = value + @"\csgo\cfg\";
                Autoexec = value + @"\csgo\cfg\autoexec.cfg";
                Video = value + @"\csgo\cfg\video.txt";
            }
        }

        public static string CsgoExe { get; private set; }
        public static string CfgFolder { get; private set; }
        public static string Autoexec { get; private set; }
        public static string Video { get; private set; }

        // Check if csgo is installed
        private static void validateCsgoPath(string path)
        {
            if (!File.Exists(path + @"\csgo.exe"))
            {
                throw new Exception("csgo.exe was not found in: " + path);
            }
        }

        private static void validateSteamPath(string path)
        {
            if (!Directory.Exists(path + @"\userdata"))
            {
                throw new Exception("userdata folder was not found in: " + path);
            }
        }

        // Check if csgo is installed in the steamapps folder
        private static void checkIfCsgoIsLocatedWithin(string path)
        {
            if (Directory.Exists(path + @"\SteamApps\common\Counter-Strike Global Offensive"))
            {
                Csgo = path + @"\SteamApps\common\Counter-Strike Global Offensive";
            }
        }
    }
}
