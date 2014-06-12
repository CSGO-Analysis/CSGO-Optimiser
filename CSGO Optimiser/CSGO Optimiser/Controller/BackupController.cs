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

        public string SaveBackup()
        {
            string errors = "";
            SteamController.ValidateSteamPath();
            Backup backup = new Backup(Guid.NewGuid(), DateTime.Now);

            if (!Directory.Exists("Backups"))
            {
                Directory.CreateDirectory("Backups");
            }

            string folder = "Backups\\" + backup.Id;
            Directory.CreateDirectory(folder);
            exportKey("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout",
                folder + "\\Backup_CapsLock.reg");
            exportKey("HKEY_USERS\\.DEFAULT\\Control Panel\\Mouse", folder + "\\Backup_DefaultMouseKey.reg");
            exportKey("HKEY_CURRENT_USER\\Control Panel\\Mouse", folder + "\\Backup_MouseKey.reg");
            exportKey("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers",
                folder + "\\Backup_VisualThemes.reg");
            
            string[] cfgs = new string[] { SteamPaths.CfgFolder + "\\config.cfg",
                SteamPaths.CfgFolder + "\\video.txt", SteamPaths.Autoexec };

            foreach (string cfg in cfgs)
            {
                if (File.Exists(cfg))
                {
                    File.Copy(cfg, folder + "\\" + cfg.Split('\\').Last());
                }
                else
                {
                    errors += cfg + " was not found. \n";
                }
            }

            string[] dirs = Directory.GetDirectories(SteamPaths.Steam + @"\userdata\");
            foreach (string dir in dirs)
            {
                if (File.Exists(dir + @"\config\localconfig.vdf"))
                {
                    string[] pathSplit = dir.Split('\\');
                    string accountNumber = pathSplit[pathSplit.Count() - 1];
                    File.Copy(dir + @"\config\localconfig.vdf", folder + "\\" + accountNumber + "_localconfig.vdf");
                    backup.Localconfigs.Add(accountNumber + "_localconfig.vdf");
                }
            }
            File.WriteAllLines(folder + "\\localconfigs.txt", backup.Localconfigs);

            File.WriteAllLines(folder + "\\backup.txt", backup.ToStringArray());
            backups.Add(backup);
            return errors + "Backup ("+backup.Id+") succesfully saved. \n";
        }

        public string DeleteBackup(IBackup backup)
        {
            Directory.Delete("Backups\\" + backup.Id, true);
            backups.Remove(backup);
            return "Backup (" + backup.Id + ") succesfully deleted. \n";
        }

        public string RestoreBackup(IBackup backup)
        {
            SteamController.ValidateSteamPath();
            string errors = "";
            string path = "Backups\\" + backup.Id + "\\";

            List<string> regFiles = new List<string>() { "Backup_DefaultMouseKey.reg", "Backup_CapsLock.reg",
                "Backup_MouseKey.reg", "Backup_VisualThemes.reg" };

            foreach (string regFile in regFiles)
            {
                if (File.Exists(path + regFile))
                {
                    Process p = Process.Start("regedit.exe", "/S" + path + regFile);
                    p.WaitForExit();
                }
                else
                {
                    errors += regFile + " was not found. \n";
                }
            }

            string[] cfgs = new string[] { "config.cfg", "autoexec.cfg", "video.txt" };
            foreach (string cfg in cfgs)
            {
                if (File.Exists(path + cfg))
                {
                    File.Copy(path + cfg, SteamPaths.CfgFolder + cfg, true);
                }
                else
                {
                    errors += cfg + " was not found \n";
                }
            }

            foreach (string localconfig in backup.Localconfigs)
            {
                if (File.Exists(path + localconfig))
                {
                    string accountNumber = localconfig.Split('_').First();
                    File.Copy(path + localconfig, SteamPaths.Steam + "\\userdata\\" + accountNumber + "\\config\\localconfig.vdf", true);
                }
                else
                {
                    errors += localconfig + " was not found \n";
                }
            }

            return errors + "Backup (" + backup.Id + ") succesfully restored. \n";
        }

        private void createBackups()
        {
            if (!Directory.Exists("Backups"))
            {
                Directory.CreateDirectory("Backups");
            }
            string[] backupDirs = Directory.GetDirectories("Backups\\");
            foreach (string backupDir in backupDirs)
            {
                string backupPath = backupDir + "\\backup.txt";
                if (File.Exists(backupPath))
                {
                    string[] backupTxt = File.ReadAllLines(backupPath);
                    Backup backup = new Backup(Guid.NewGuid(), DateTime.Parse("18-06-1990 13:37"));
                    foreach (string line in backupTxt)
                    {
                        if (line.Contains("Id = "))
                        {
                            Guid id;
                            Guid.TryParse(line.Split('=').Last().Replace(" ", ""), out id);
                            backup.Id = id;
                        }
                        if (line.Contains("Timestamp = "))
                        {
                            DateTime timestamp;
                            DateTime.TryParse(line.Split('=').Last(), out timestamp);
                            backup.Timestamp = timestamp;
                        }
                    }
                    if (File.Exists(backupDir + "\\localconfigs.txt"))
                    {
                        string[] localconfigs = File.ReadAllLines(backupDir + "\\localconfigs.txt");
                        foreach (string localconfig in localconfigs)
                        {
                            backup.Localconfigs.Add(localconfig);
                        }
                    }
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
