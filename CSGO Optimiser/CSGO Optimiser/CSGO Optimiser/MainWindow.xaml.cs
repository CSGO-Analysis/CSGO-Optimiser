using Common;
using Controller;
using CSGO_Optimiser.UserControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace CSGO_Optimiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Optimisation optimisation;
        private Backup backup;
        private BackupController backupController;

        public MainWindow()
        {
            InitializeComponent();
            backupController = new BackupController();
            optimisation = new Optimisation(backupController);
            backup = new Backup(backupController);
            optimisationUserControl.Content = optimisation;
            backupUserControl.Content = backup;
        }

        private void mainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SteamController.GetSteamPath() != null)
            {
                optimisation.pathLabel.Content = SteamController.GetSteamPath();
                backup.pathLabel.Content = SteamController.GetSteamPath();
            }
        }
    }
}
