using Controller;
using CSGO_Optimiser.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CSGO_Optimiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Optimisation optimisation;
        private Backup backup;
        private About about;
        private BackupController backupController;
        private VersionController versionController;
        private Version curVersion;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "CSGO Optimiser";
            curVersion = new Version(0,3);
            backupController = new BackupController();
            versionController = new VersionController();
            optimisation = new Optimisation(backupController);
            backup = new Backup(backupController);
            about = new About(curVersion);
            optimisationUserControl.Content = optimisation;
            backupUserControl.Content = backup;
            aboutUserControl.Content = about;
            versionCheck();
        }

        // When tabcontrolChanged set SteamPath in Backup and Optimisation UserControls if it is different from null
        private void mainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SteamController.GetSteamPath() != null)
            {
                optimisation.steamPathLabel.Content = SteamController.GetSteamPath();
                optimisation.csgoPathLabel.Content = SteamController.GetCsgoPath();
                backup.steamPathLabel.Content = SteamController.GetSteamPath();
                backup.csgoPathLabel.Content = SteamController.GetCsgoPath();
            }
        }

        private void versionCheck()
        {
            Version newVersion = versionController.GetNewestVersion();
            if (curVersion.CompareTo(newVersion) < 0)
            {
                string title = "New version detected.";
                string question = "Open download page for the new version?";
                if (MessageBoxResult.Yes ==
                 MessageBox.Show(this, question, title,
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question))
                {
                    System.Diagnostics.Process.Start("https://sourceforge.net/projects/csgo-optimiser/files/");
                    this.Close();
                }
            }  
        }

    }
}
