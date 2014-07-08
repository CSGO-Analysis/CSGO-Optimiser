using Common;

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
        public string FolderPath { get; set; }

        public Profile(string name, string config, string crosshair, string autoexec, string videoSettings,
            string launchOptions, string nvidiaProfile, string folderPath)
        {
            Name = name;
            Config = config;
            Crosshair = crosshair;
            Autoexec = autoexec;
            VideoSettings = videoSettings;
            LaunchOptions = launchOptions;
            NvidiaProfile = nvidiaProfile;
            FolderPath = folderPath;
        }

        public Profile()
        {
        }
    }
}
