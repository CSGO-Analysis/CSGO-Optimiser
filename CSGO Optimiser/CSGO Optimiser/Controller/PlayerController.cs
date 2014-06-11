using Common;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class PlayerController
    {
        private List<IPlayer> players;

        public List<IPlayer> GetPlayers()
        {
            if (players == null)
            {
                players = new List<IPlayer>();
                createPlayers();
            }
            return players;
        }

        private void createPlayers()
        {
            string[] playerDirs = Directory.GetDirectories(@"Resources\Players\");
            foreach (string playerDir in playerDirs)
            {
                string settingsPath = playerDir + "\\settings.txt";
                if (File.Exists(settingsPath))
                {
                    string name = "", config = "", crosshair = "", autoexec = "", videoSettings = "", launchOptions = "", nvidiaProfile = "";
                    bool disabledMouseAcc = false, disabledIngameMouseAcc = false, disabledCapsLock = false, disabledVisualThemes = false;

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
                        if (line.Contains("DisabledMouseAcc = "))
                        {
                            string mouseAccString = line.Split('=').Last().Replace(" ", "");
                            bool.TryParse(mouseAccString, out disabledMouseAcc);
                        }
                        if (line.Contains("DisabledIngameMouseAcc = "))
                        {
                            string ingameMouseAccString = line.Split('=').Last().Replace(" ", "");
                            bool.TryParse(ingameMouseAccString, out disabledIngameMouseAcc);
                        }
                        if (line.Contains("DisabledCapsLock = "))
                        {
                            string capsLockString = line.Split('=').Last().Replace(" ", "");
                            bool.TryParse(capsLockString, out disabledCapsLock);
                        }
                        if (line.Contains("DisabledVisualThemes = "))
                        {
                            string visualThemesString = line.Split('=').Last().Replace(" ", "");
                            bool.TryParse(visualThemesString, out disabledVisualThemes);
                        }
                    }
                    Player player = new Player(name, config, crosshair, autoexec, videoSettings, launchOptions, nvidiaProfile,
                        disabledMouseAcc, disabledIngameMouseAcc, disabledCapsLock, disabledVisualThemes, playerDir + "\\");
                    players.Add(player);
                }
            }
        }
    }
}
