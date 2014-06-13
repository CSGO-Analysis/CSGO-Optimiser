using Common;
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
        public string CopyAutoexec(IProfile profile)
        {
            File.Copy(profile.FolderPath + profile.Autoexec, SteamPaths.Autoexec, true);
            return profile.Autoexec + " succesfully created. \n";
        }

        public string CopyProfileConfig(IProfile profile)
        {
            string destCfg = SteamPaths.CfgFolder + profile.Config;
            File.Copy(profile.FolderPath + profile.Config, destCfg, true);

            List<string> autoexec;
            if (File.Exists(SteamPaths.Autoexec))
            {
                autoexec = File.ReadAllLines(SteamPaths.Autoexec).ToList();
                if (!autoexec.Contains("exec " + profile.Config))
                {
                    autoexec.Add("exec " + profile.Config);
                }
            }
            else
            {
                autoexec = new List<string>() { "exec " + profile.Config };
            }
            File.WriteAllLines(SteamPaths.Autoexec, autoexec);
            return profile.Config + " succesfully created. \n";
        }

        public string CopyProfileCrosshair(IProfile profile)
        {
            string destCfg = SteamPaths.CfgFolder + profile.Crosshair;
            File.Copy(profile.FolderPath + profile.Crosshair, destCfg, true);

            List<string> autoexec;
            if (File.Exists(SteamPaths.Autoexec))
            {
                autoexec = File.ReadAllLines(SteamPaths.Autoexec).ToList();
                if (!autoexec.Contains("exec " + profile.Crosshair))
                {
                    autoexec.Add("exec " + profile.Crosshair);
                }
            }
            else
            {
                autoexec = new List<string>() { "exec " + profile.Crosshair };
            }
            File.WriteAllLines(SteamPaths.Autoexec, autoexec);
            return profile.Crosshair + " succesfully created. \n";
        }

        public string CopyVideoConfig(IProfile profile)
        {
            File.Copy(profile.FolderPath + profile.VideoSettings, SteamPaths.Video, true);
            return profile.VideoSettings + " succesfully created. \n";
        }

        public string SetLaunchOptions(IProfile profile)
        {
            //uint maxRefreshRate = findMaxRefreshRate();

            string[] dirs = Directory.GetDirectories(SteamPaths.Steam + @"\userdata\");
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
                            localconfig.Insert(k, "\t\t\t\t\t\t\"LaunchOptions\"\t\"" + profile.LaunchOptions);
                            File.WriteAllLines(dir + @"\config\localconfig.vdf", localconfig);
                            return string.Format("Launch options:" + profile.LaunchOptions + " succesfully added. \n");
                        }
                    }
                }
            }
            return null;
        }

        public string SetNvidiaSettings(IProfile profile)
        {
            Process p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = @"Resources\nvidiaInspector.exe";
            p.StartInfo.Arguments = profile.FolderPath + profile.NvidiaProfile;
            p.Start();
            p.WaitForExit();

            return "nvidiaInspector finished importing " + profile.NvidiaProfile + ". \n";
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
            return "Mouse Acceleration succesfully disabled (" + dpi + "% monitor size). \n";
        }

        public string DisableIngameMouseAcc()
        {
            string ingameAcc = SteamPaths.CfgFolder + "IngameMouseAccelOff.cfg";
            File.Copy(@"Resources\IngameMouseAccelOff.cfg", ingameAcc, true);


            List<string> autoexec;
            if (File.Exists(SteamPaths.Autoexec))
            {
                autoexec = File.ReadAllLines(SteamPaths.Autoexec).ToList();
                if (!autoexec.Contains("exec IngameMouseAccelOff.cfg"))
                {
                    autoexec.Add("exec IngameMouseAccelOff.cfg");
                }
            }
            else
            {
                autoexec = new List<string>() { "exec IngameMouseAccelOff.cfg" };
            }
            File.WriteAllLines(SteamPaths.Autoexec, autoexec);

            return "Ingame Mouse Commands succesfully applied. \n";
        }

        public string DisableCapsLock()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layout", true);
            if (key == null)
            {
                throw new InvalidOperationException(@"Cannot open registry key HKLM\SYSTEM\CurrentControlSet\Control\Keyboard Layout.");
            }
            key.SetValue("Scancode Map", new byte[] {00,00,00,00,00,00,00,00,0x02,00,00,00,0x64,00,0x3a,00,00,00,00,00}, RegistryValueKind.Binary);
            return "CapsLock succesfully disabled. \n";
        }

        public string DisableVisualThemes()
        {
            if (SteamPaths.CsgoExe != null)
            {
                var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true);
                if (key == null)
                {
                    throw new InvalidOperationException(@"Cannot open registry key HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers.");
                }
                key.SetValue(SteamPaths.CsgoExe, "DISABLETHEMES");

                return "Visual themes succesfully disabled for csgo.exe. \n";
            }
            else
            {
                throw new Exception("CSGO folder was not found. Disable Visual Themes unsuccessful.");
            }
        }

        //private uint findMaxRefreshRate()
        //{
        //    var scope = new System.Management.ManagementScope();
        //    var q = new System.Management.ObjectQuery("SELECT * FROM CIM_VideoControllerResolution");

        //    using (var searcher = new System.Management.ManagementObjectSearcher(scope, q))
        //    {
        //        var results = searcher.Get();
        //        UInt32 maxHZ = 0;

        //        foreach (var item in results)
        //        {
        //            if ((UInt32)item["RefreshRate"] > maxHZ)
        //                maxHZ = (UInt32)item["RefreshRate"];
        //        }
        //        if (maxHZ >= 143)
        //        {
        //            return 144;
        //        }
        //        else if (maxHZ >= 119)
        //        {
        //            return 120;
        //        }
        //        else if (maxHZ >= 74)
        //        {
        //            return 75;
        //        }
        //        else
        //        {
        //            return 60;
        //        }
        //    }
        //}

    }
}
