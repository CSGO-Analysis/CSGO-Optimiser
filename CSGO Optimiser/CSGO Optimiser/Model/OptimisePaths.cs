using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OptimisePaths
    {
        private string _steam;

        public string Steam
        {
            get { return _steam; }
            set
            {
                validateCsgoPath(value);
                _steam = value;
                CsgoExe = value + @"\SteamApps\common\Counter-Strike Global Offensive\csgo.exe";
                Cfg = value + @"\SteamApps\common\Counter-Strike Global Offensive\csgo\cfg\";
            }
        }

        public string CsgoExe { get; private set; }
        public string Cfg { get; private set; }

        private void validateCsgoPath(string path)
        {
            if (!Directory.Exists(path + @"\SteamApps\common\Counter-Strike Global Offensive"))
            {
                throw new Exception("Counter-Strike Global Offensive folder was not found.");
            }
        }
    }
}
