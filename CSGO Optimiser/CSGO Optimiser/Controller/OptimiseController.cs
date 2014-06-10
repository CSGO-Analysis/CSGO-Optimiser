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
            var process = Process.Start(@"Resources\Geforce 3D Profile Manager.exe");
            process.WaitForExit();
            TimeSpan time = process.ExitTime - process.StartTime;
            return "Geforce 3D Profile Manager exited after " + time.Seconds.ToString() + " seconds. \n";
        }
        
        public string CopyAutoexec()
        {
            string autoexec = optimisePaths.Cfg + "autoexec.cfg";
            File.Copy(@"Resources\autoexec.cfg", autoexec, true);
            return autoexec + " succesfully created. \n";
        }

        public string CopyVideoSettings()
        {
            string video = optimisePaths.Cfg + "video.txt";
            File.Copy(@"Resources\video.txt", video, true);
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
            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layout", true);
            if (key == null)
            {
                throw new InvalidOperationException(@"Cannot open registry key HKLM\SYSTEM\CurrentControlSet\Control\Keyboard Layout.");
            }
            key.SetValue("Scancode Map", new byte[] {00,00,00,00,00,00,00,00,02,00,00,00,0x64,00,0x3a,00,00,00,00,00}, RegistryValueKind.Binary);
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

        public string DisableIngameAcc()
        {
            string ingameAcc = optimisePaths.Cfg + "IngameMouseAccelOff.cfg";
            File.Copy(@"Resources\IngameMouseAccelOff.cfg", ingameAcc, true);
            List<string> autoexec = File.ReadAllLines(optimisePaths.Cfg + "autoexec.cfg").ToList();
            if (!autoexec.Contains("exec IngameMouseAccelOff.cfg"))
            {
                autoexec.Add("exec IngameMouseAccelOff.cfg");
            }
            File.WriteAllLines(optimisePaths.Cfg + "autoexec.cfg", autoexec);

            return "Ingame Acceleration succesfully disabled. \n";
        }
    }
}
