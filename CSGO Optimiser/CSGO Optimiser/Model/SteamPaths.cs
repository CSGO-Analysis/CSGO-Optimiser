using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class SteamPaths
    {
        private static string _steam;

        public static string Steam
        {
            get { return _steam; }
            set
            {
                validateCsgoPath(value);
                _steam = value;
                CsgoExe = value + @"\SteamApps\common\Counter-Strike Global Offensive\csgo.exe";
                CfgFolder = value + @"\SteamApps\common\Counter-Strike Global Offensive\csgo\cfg\";
                Autoexec = value + @"\SteamApps\common\Counter-Strike Global Offensive\csgo\cfg\autoexec.cfg";
                Video = value + @"\SteamApps\common\Counter-Strike Global Offensive\csgo\cfg\video.txt";
            }
        }

        public static string CsgoExe { get; private set; }
        public static string CfgFolder { get; private set; }
        public static string Autoexec { get; private set; }
        public static string Video { get; private set; }

        private static void validateCsgoPath(string path)
        {
            if (!Directory.Exists(path + @"\SteamApps\common\Counter-Strike Global Offensive"))
            {
                throw new Exception("Counter-Strike Global Offensive folder was not found.");
            }
        }
    }
}
