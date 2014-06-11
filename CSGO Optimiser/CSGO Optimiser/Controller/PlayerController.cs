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

        public string CopyPlayerConfig(IPlayer player)
        {
            throw new NotImplementedException();

            //string playercfg = SteamPaths.CfgFolder + player.Name + ".cfg";
            //File.Copy(player.CfgPath, playercfg, true);

            //List<string> autoexec;
            //if (File.Exists(SteamPaths.Autoexec))
            //{
            //    autoexec = File.ReadAllLines(SteamPaths.Autoexec).ToList();
            //    if (!autoexec.Contains(string.Format("exec {0}.cfg", player.Name)))
            //    {
            //        autoexec.Add(string.Format("exec {0}.cfg", player.Name));
            //    }
            //}
            //else
            //{
            //    autoexec = new List<string>() { string.Format("exec {0}.cfg", player.Name) };
            //}
            //File.WriteAllLines(SteamPaths.Autoexec, autoexec);
            //return playercfg + " succesfully created. \n";
        }

        private void createPlayers()
        {
            string[] playerDirs = Directory.GetDirectories(@"Resources\Players\");
            foreach (string play in playerDirs)
            {
                string settingsPath = play + "\\settings.txt";
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
                    Player player = new Player(name, config, crosshair, autoexec, videoSettings,
                        launchOptions, nvidiaProfile, disabledMouseAcc, disabledIngameMouseAcc, disabledCapsLock, disabledVisualThemes);
                    players.Add(player);
                }
            }


                //string name = play.Split('\\').Last();
                //string cfgPath;
                //string videoPath;
                //if (File.Exists(string.Format(@"{0}\{1}.cfg", play, name)))
                //{
                //    cfgPath = string.Format(@"{0}\{1}.cfg", play, name);
                //}
                //else
                //{
                //    break;
                //}

                //if (File.Exists(play + @"\video.txt"))
                //{
                //    videoPath = play + @"\video.txt";
                //}
                //else
                //{
                //    videoPath = "Resources\video.txt";
                //}

                //Player player = new Player(name, cfgPath, videoPath, );
                //players.Add(player);
        }
    }
}
