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
            string[] dirs = Directory.GetDirectories(optimisePaths.Steam + @"\userdata\");
            foreach (string dir in dirs)
            {
                if (File.Exists(dir + @"\config\localconfig.vdf"))
                {
                    List<string> localconfig = File.ReadAllLines(dir + @"\config\localconfig.vdf").ToList();
                    for (int i = 0; i < localconfig.Count(); i++)
                    {
                        if (localconfig[i] == "\t\t\t\t\t\"730\"")
                        {
                            for (int j = i; j < localconfig.Count(); j++)
                            {
                                if (localconfig[j] == "\t\t\t\t\t}")
                                {
                                    break;
                                }
                                else if (localconfig[j].Contains("LaunchOptions"))
                                {
                                    localconfig.Remove(localconfig[j]);
                                }
                            }
                            int k = i + 2;
                            localconfig.Insert(k, "\t\t\t\t\t\t\"LaunchOptions\"\t\"-console -freq 120 -novid +exec autoexec.cfg -high\"");
                            File.WriteAllLines(dir + @"\config\localconfig.vdf", localconfig);
                            return "Launch Options succesfully added. \n";
                        }
                    }
                }
            }
            return null;
        }
        
        public string DisableMouseAcc()
        {
            int dpi = 0;
            var dpiKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            if (dpiKey == null)
            {
                throw new InvalidOperationException(@"Cannot open registry key HKCU\Control Panel\Desktop");
            }
            else
            {
                var defaultMouseKey = Registry.Users.OpenSubKey(@".DEFAULT\Control Panel\Mouse", true);
                if (defaultMouseKey == null)
                {
                    throw new InvalidOperationException(@"Cannot open registry key HKCU\Control Panel\Mouse");
                }
                else
                {
                    defaultMouseKey.SetValue("MouseSpeed", "0", RegistryValueKind.String);
                    defaultMouseKey.SetValue("MouseThreshold1", "0", RegistryValueKind.String);
                    defaultMouseKey.SetValue("MouseThreshold2", "0", RegistryValueKind.String);
                }
                
                var mouseKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse", true);
                if (mouseKey == null)
                {
                    throw new InvalidOperationException(@"Cannot open registry key HKCU\Control Panel\Mouse");
                }
                else
                {
                    mouseKey.SetValue("MouseSensitivity", "10", RegistryValueKind.String);
                    mouseKey.SetValue("SmoothMouseYCurve", new byte[] { 00,00,00,00,00,00,00,00,00,00,0x38,00,00,00,00,00,
                        00,00,0x70,00,00,00,00,00,00,00,0xA8,00,00,00,00,00,00,00,0xE0,00,00,00,00,00 }, RegistryValueKind.Binary);

                    if (dpiKey.GetValue("LogPixels") == null || (dpiKey.GetValue("LogPixels").ToString() == "96"))
                    {
                        dpi = 100;
                        mouseKey.SetValue("SmoothMouseXCurve", new byte[] {00,00,00,00,00,00,00,00,0x70,0x3D,0x0A,00,00,00,00,00,
                            0xE0,0x7A,0x14,00,00,00,00,00,0x50,0xB8,0x1E,00,00,00,00,00,0xC0,0xF5,0x28,00,00,00,00,00}, RegistryValueKind.Binary);
                    }
                    else if (dpiKey.GetValue("LogPixels").ToString() == "120")
                    {
                        dpi = 120;
                        mouseKey.SetValue("SmoothMouseXCurve", new byte[] {	00,00,00,00,00,00,00,00,0xC0,0xCC,0x0C,00,00,00,00,00,
                            0x80,0x99,0x19,00,00,00,00,00,0x40,0x66,0x26,00,00,00,00,00,00,0x33,0x33,00,00,00,00,00 }, RegistryValueKind.Binary);
                    }
                    else if (dpiKey.GetValue("LogPixels").ToString() == "144")
                    {
                        dpi = 150;
                        mouseKey.SetValue("SmoothMouseXCurve", new byte[] { 00,00,00,00,00,00,00,00,0x20,0x5C,0x0F,00,00,00,00,00,
                            0x40,0xB8,0x1E,00,00,00,00,00,0x60,0x14,0x2E,00,00,00,00,00,0x80,0x70,0x3D,00,00,00,00,00 }, RegistryValueKind.Binary);
                    }
                }
            }
            return "Mouse Acceleration succesfully disabled (" + dpi + "% dpi). \n";
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
