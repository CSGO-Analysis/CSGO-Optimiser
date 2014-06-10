using Controller;
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
        private OptimiseController optimiseController;

        public MainWindow()
        {
            InitializeComponent();
            optimiseController = new OptimiseController();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.ShowDialog();
                optimiseController.SetSteamPath(dialog.SelectedPath);
                pathLabel.Content = optimiseController.GetSteamPath();
                optimiseButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void optimiseButton_Click(object sender, RoutedEventArgs e)
        {
            if (optimiseController.GetSteamPath() != null)
            {
                int changes = 0;
                try
                {
                    if (nvidiaCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.SetNvidiaSettings();
                        changes++;
                    }
                    if (autoexecCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.CopyAutoexec();
                        changes++;
                    }
                    if (ingameVideoButton.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.CopyVideoSettings();
                        changes++;
                    }
                    if (launchOptionsCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.SetLaunchOptions();
                        changes++;
                    }
                    if (mouseAccCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.DisableMouseAcc();
                        changes++;
                    }
                    if (capsLockButton.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.DisableCapsLock();
                        changes++;
                    }
                    if (visualThemesCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.DisableVisualThemes();
                        changes++;
                    }
                    logTextBox.Text += string.Format("\nOptimisation finished ({0} changes) \n", changes);
                    logTextBox.ScrollToEnd();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please locate your Steam folder");
            }
        }
    }
}
