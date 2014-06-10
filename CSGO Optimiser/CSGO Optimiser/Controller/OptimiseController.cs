using Microsoft.Win32;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Controller
{
    public class OptimiseController
    {
        private OptimisePaths optimisePaths;

        public OptimiseController()
        {
            optimisePaths = new OptimisePaths();
        }

        public void SetSteamPath(string selectedPath)
        {
            optimisePaths.Steam = selectedPath;
        }

        public string GetSteamPath()
        {
            return optimisePaths.Steam;
        }

        public string GetCfgPath()
        {
            return optimisePaths.Cfg;
        }

        public string SetNvidiaSettings()
        {
            var process = Process.Start("Geforce 3D Profile Manager.exe");
            process.WaitForExit();
            TimeSpan time = process.ExitTime - process.StartTime;
            return "Geforce 3D Profile Manager exited after " + time.Seconds.ToString() + " seconds. \n";
        }
        
        public string CopyAutoexec()
        {
            string autoexec = optimisePaths.Cfg + "autoexec.cfg";
            File.Copy("autoexec.cfg", autoexec, true);
            return autoexec + " succesfully created. \n";
        }

        public string CopyVideoSettings()
        {
            string video = optimisePaths.Cfg + "video.txt";
            File.Copy("video.txt", video, true);
            return video + " succesfully created. \n";
        }

        public string SetLaunchOptions()
        {
            /*string[] dirs = Directory.GetDirectories(optimisePaths.Steam + @"\userdata\");
            foreach (string dir in dirs)
            {
                if (File.Exists(dir + @"\config\localconfig.vdf"))
                {
                    IEnumerable<string> csgoLines = File.ReadLines(dir + @"\config\localconfig.vdf")
                    .SkipWhile(line => !line.Equals("\t\t\t\t\t\"730\""))
                    .TakeWhile(line => !line.Equals("\t\t\t\t\t}"));

                    if (csgoLines.Contains("LaunchOptions"))
                    {
                        File.AppendAllText(dir + @"\config\localconfig.vdf", "test");
                        // edit launch options
                    }
                    else
                    {
                        // add launch options on new line
                    }
                }
            }*/
            throw new NotImplementedException();
        }
        
        public string DisableMouseAcc()
        {
            //Process.Start("EnhancedPointerPrecOff.reg");
            System.Drawing.Size resolution = System.Windows.Forms.SystemInformation.PrimaryMonitorSize;
            return resolution.ToString();
        }

        public string DisableCapsLock()
        {
            Process.Start("KillCapsLock.reg");
            return "CapsLock succesfully disabled. \n";
        }

        public string DisableVisualThemes()
        {
            if (optimisePaths.CsgoExe != null)
            {
                var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true);
                if (key == null)
                {
                    throw new InvalidOperationException(@"Cannot open registry key HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers.");
                }
                key.SetValue(optimisePaths.CsgoExe, "DISABLETHEMES");

                return "DISABLETHEMES key succesfully added in " + key.Name + ". \n";
            }
            else
            {
                throw new Exception("CSGO folder was not found.");
            }
        }
    }
}
