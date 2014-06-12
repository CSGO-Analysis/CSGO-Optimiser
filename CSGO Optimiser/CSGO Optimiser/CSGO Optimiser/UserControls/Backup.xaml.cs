using Common;
using Controller;
using System;
using System.Collections.Generic;
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

        public Backup()
        {
            InitializeComponent();
            backupController = new BackupController();
            updateGUI();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                backupController.SaveBackup();
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
            if (MessageBox.Show("Are you sure you want to delete the selected backup(s)?",
                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (IBackup selectedBackup in backupsListView.SelectedItems)
                {
                    backupController.DeleteBackup(selectedBackup);
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
    }
}
