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

        public Backup(SteamBrowse sBrowse)
        {
            InitializeComponent();
            backupController = new BackupController();
            steamBrowse.Content = sBrowse;
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
            foreach (IBackup selectedBackup in backupsListView.SelectedItems)
            {
                //IBackup selectedBackup = (IBackup)backupsListView.SelectedItem;
                backupController.DeleteBackup(selectedBackup);
            }
            updateGUI();
        }
    }
}
