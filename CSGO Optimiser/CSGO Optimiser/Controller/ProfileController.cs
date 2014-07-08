using Common;
using Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Controller
{
    public class ProfileController
    {
        private List<IProfile> profiles;

        // Return profiles. If null run createProfiles()
        public List<IProfile> GetProfiles()
        {
            if (profiles == null)
            {
                profiles = new List<IProfile>();
                createProfiles();
            }
            return profiles;
        }

        // Find all directories in profiles folder, and read their settings.txt. Add profile to profiles.
        private void createProfiles()
        {
            string[] profileDirs = Directory.GetDirectories(@"Resources\Profiles\");
            foreach (string profileDir in profileDirs)
            {
                string settingsPath = profileDir + "\\settings.txt";
                if (File.Exists(settingsPath))
                {
                    string name = "", config = "", crosshair = "", autoexec = "", videoSettings = "", launchOptions = "", nvidiaProfile = "";

                    string[] settings = File.ReadAllLines(settingsPath);
                    foreach (string line in settings)
                    {
                        if (line.Contains("Name = "))
                        {
                            name = line.Split('=').Last().Replace(" ", "");
                        }
                        if (line.Contains("Config = "))
                        {
                            config = line.Split('=').Last().Replace(" ", "");
                        }
                        if (line.Contains("Crosshair = "))
                        {
                            crosshair = line.Split('=').Last().Replace(" ", "");
                        }
                        if (line.Contains("Autoexec = "))
                        {
                            autoexec = line.Split('=').Last().Replace(" ", "");
                        }
                        if (line.Contains("VideoSettings = "))
                        {
                            videoSettings = line.Split('=').Last().Replace(" ", "");
                        }
                        if (line.Contains("LaunchOptions = "))
                        {
                            launchOptions = line.Split('=').Last();
                        }
                        if (line.Contains("NvidiaProfile = "))
                        {
                            nvidiaProfile = line.Split('=').Last().Replace(" ", "");
                        }
                    }
                    Profile profile = new Profile(name, config, crosshair, autoexec, videoSettings, launchOptions, nvidiaProfile, profileDir + "\\");
                    profiles.Add(profile);
                }
            }
        }
    }
}
