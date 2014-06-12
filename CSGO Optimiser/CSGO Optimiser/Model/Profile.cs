using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Profile : IProfile
    {
        public string Name { get; set; }
        public string Config { get; set; }
        public string Crosshair { get; set; }
        public string Autoexec { get; set; }
        public string VideoSettings { get; set; }
        public string LaunchOptions { get; set; }
        public string NvidiaProfile { get; set; }
        public bool DisabledMouseAcc { get; set; }
        public bool DisabledIngameMouseAcc { get; set; }
        public bool DisabledCapsLock { get; set; }
        public bool DisabledVisualThemes { get; set; }
        public string FolderPath { get; set; }

        public Profile(string name, string config, string crosshair, string autoexec, string videoSettings,
            string launchOptions, string nvidiaProfile, bool disabledMouseAcc, bool disabledIngameMouseAcc, bool disabledCapsLock, bool disabledVisualThemes,
            string folderPath)
        {
            Name = name;
            Config = config;
            Crosshair = crosshair;
            Autoexec = autoexec;
            VideoSettings = videoSettings;
            LaunchOptions = launchOptions;
            NvidiaProfile = nvidiaProfile;
            DisabledMouseAcc = disabledMouseAcc;
            DisabledIngameMouseAcc = disabledIngameMouseAcc;
            DisabledCapsLock = disabledCapsLock;
            DisabledVisualThemes = disabledVisualThemes;
            FolderPath = folderPath;
        }

        public Profile()
        {
        }
    }
}
