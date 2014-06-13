using Common;
using Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSGO_Optimiser.UserControls
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : UserControl
    {
        private BackupController backupController;

        public Backup(BackupController bkController)
        {
            InitializeComponent();
            backupController = bkController;
            updateGUI();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logTextBox.Text += backupController.SaveBackup();
                logTextBox.ScrollToEnd();
                updateGUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateGUI()
        {
            backupsListView.ItemsSource = null;
            backupsListView.ItemsSource = backupController.GetBackups();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (backupsListView.SelectedItem != null && MessageBox.Show("Are you sure you want to delete the selected backup(s)?",
                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (IBackup selectedBackup in backupsListView.SelectedItems)
                {
                    try
                    {
                        logTextBox.Text += backupController.DeleteBackup(selectedBackup);
                        logTextBox.ScrollToEnd();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                updateGUI();
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.ShowDialog();
                SteamController.SetSteamPath(dialog.SelectedPath);
                pathLabel.Content = SteamController.GetSteamPath();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (backupsListView.SelectedItem != null && MessageBox.Show("Are you sure you want to restore your settings to the selected backup?" +
                "\nThis action will restart Steam if opened.", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    bool restartSteam = false;
                    Process[] steamProcess = Process.GetProcessesByName("Steam");
                    if (steamProcess.Length != 0)
                    {
                        steamProcess[0].Kill();
                        steamProcess[0].WaitForExit();
                        restartSteam = true;
                    }
                    IBackup selectedBackup = (IBackup)backupsListView.SelectedItem;
                    logTextBox.Text += backupController.RestoreBackup(selectedBackup);
                    logTextBox.ScrollToEnd();
                    if (MessageBox.Show("Settings succesfully restored.\nYou must reboot in order to apply the registry changes. Reboot now?", "Success",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                    else if (restartSteam == true)
                    {
                        Process.Start(SteamController.GetSteamPath() + "\\Steam.exe");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
