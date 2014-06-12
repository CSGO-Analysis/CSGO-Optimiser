using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class BackupController
    {
        private List<IBackup> backups;

        public List<IBackup> GetBackups()
        {
            if (backups == null)
            {
                backups = new List<IBackup>();
                createBackups();
            }
            return backups;
        }

        public void SaveBackup()
        {
            Backup backup = new Backup(Guid.NewGuid(), DateTime.Now, "config", "crosshair", "autoexec", "videosettings", "launchoptions");

            string folder = "Backups\\" + backup.Id;
            Directory.CreateDirectory(folder);
            exportKey("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout",
                folder + "\\Backup_CapsLock.reg");
            exportKey("HKEY_USERS\\.DEFAULT\\Control Panel\\Mouse", folder + "\\Backup_DefaultMouseKey.reg");
            exportKey("HKEY_CURRENT_USER\\Control Panel\\Mouse", folder + "\\Backup_MouseKey.reg");
            exportKey("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers",
                folder + "\\Backup_VisualThemes.reg");

            File.Copy(SteamPaths.CfgFolder + "config.cfg", folder + "\\config.cfg");
            // copy config etc..

            File.WriteAllLines(folder + "\\backup.txt", backup.TxtFile());
            backups.Add(backup);
        }

        public void DeleteBackup(IBackup backup)
        {
            Directory.Delete("Backups\\" + backup.Id, true);
            backups.Remove(backup);
        }

        private void createBackups()
        {
            string[] backupDirs = Directory.GetDirectories("Backups\\");
            foreach (string backupDir in backupDirs)
            {
                string backupPath = backupDir + "\\backup.txt";
                if (File.Exists(backupPath))
                {
                    string[] backupTxt = File.ReadAllLines(backupPath);
                    Guid id = Guid.NewGuid();
                    DateTime timestamp = DateTime.Parse("18-06-1990 13:37");
                    string config = "", crosshair = "", autoexec = "", videoSettings = "", launchOptions = "";
                    foreach (string line in backupTxt)
                    {
                        if (line.Contains("Id = "))
                        {
                            string idString = line.Split('=').Last().Replace(" ", "");
                            Guid.TryParse(idString, out id);
                        }
                        if (line.Contains("Timestamp = "))
                        {
                            DateTime.TryParse(line.Split('=').Last(), out timestamp);
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
                        //if (line.Contains("NvidiaProfile = "))
                        //{
                        //    backup.NvidiaProfile = line.Split('=').Last().Replace(" ", "");
                        //}
                    }
                    Backup backup = new Backup(id, timestamp, config, crosshair, autoexec, videoSettings, launchOptions);
                    backups.Add(backup);
                }
            }
        }

        private void exportKey(string regKey, string savePath)
        {
            string path = "\"" + savePath + "\"";
            string key = "\"" + regKey + "\"";

            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;
                proc = Process.Start("regedit.exe", "/e " + path + " " + key + "");

                if (proc != null) proc.WaitForExit();
            }
            finally
            {
                if (proc != null) proc.Dispose();
            }
        }
    }
}
