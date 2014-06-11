using Common;
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
        private PlayerController playerController;

        public MainWindow()
        {
            InitializeComponent();
            optimiseController = new OptimiseController();
            playerController = new PlayerController();
            playersComboBox.ItemsSource = playerController.GetPlayers();
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
            int changes = 0;
            try
            {
                if (playersComboBox.SelectedItem != null)
                {
                    validateSteamPath();
                    IPlayer player = (IPlayer) playersComboBox.SelectedItem;
                    logTextBox.Text += playerController.CopyPlayerConfig(player);
                }
                if (launchOptionsCheckBox.IsChecked == true)
                {
                    validateSteamPath();
                    Process[] steamProcess = Process.GetProcessesByName("Steam");
                    if (steamProcess.Length != 0)
                    {
                        if (MessageBox.Show("Steam must be closed in order to add launch options. Shutdown Steam?",
                            "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            steamProcess[0].Kill();
                            steamProcess[0].WaitForExit();
                            logTextBox.Text += optimiseController.SetLaunchOptions();
                            changes++;
                        }
                        else
                        {
                            logTextBox.Text += "Launch options was not added. \n";
                        }
                    }
                    else
                    {
                        logTextBox.Text += optimiseController.SetLaunchOptions();
                        changes++;
                    }
                }
                if (autoexecCheckBox.IsChecked == true)
                {
                    validateSteamPath();
                    logTextBox.Text += optimiseController.CopyAutoexec();
                    changes++;
                }
                if (videoSettingsCheckBox.IsChecked == true)
                {
                    validateSteamPath();
                    logTextBox.Text += optimiseController.CopyVideoSettings();
                    changes++;
                }
                if (mouseAccCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableMouseAcc();
                    changes++;
                }
                if (ingameMouseAccCheckBox.IsChecked == true)
                {
                    validateSteamPath();
                    logTextBox.Text += optimiseController.DisableIngameAcc();
                    changes++;
                }
                if (capsLockCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableCapsLock();
                    changes++;
                }
                if (visualThemesCheckBox.IsChecked == true)
                {
                    validateSteamPath();
                    logTextBox.Text += optimiseController.DisableVisualThemes();
                    changes++;
                }
                if (nvidiaProfileCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.SetNvidiaSettings();
                    changes++;
                }
                logTextBox.Text += string.Format("Optimisation finished ({0} changes) \n", changes);
                logTextBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (sender.Equals(nvidiaProfileCheckBox))
            {
                descriptionTextBox.Text = @"NVIDIA CSGO Profile:

Uses nvidiaInspector to import a NVIDIA 3D profile that changes the driver settings for csgo.exe only.";
            }
            else if (sender.Equals(autoexecCheckBox))
            {
                descriptionTextBox.Text = @"Autoexec.cfg:

Adds an autoexec.cfg with the following commands:
**NOTE THAT AUTOEXEC.CFG WILL BE OVERWRITTEN**

rate 128000
fps_max 999
cl_interp 0
cl_interp_ratio 1
cl_updaterate 128
cl_cmdrate 128
cl_forcepreload 1
mat_queue_mode 2";
            }
            else if (sender.Equals(videoSettingsCheckBox))
            {
                descriptionTextBox.Text = @"Advanced Ingame Video Settings:

Turns off most of the 'Advanced video settings' in the game options for high performance.

(Highly recommended to also use the NVIDIA CSGO Profile to avoid settings being overruled)";
            }
            else if (sender.Equals(launchOptionsCheckBox))
            {
                descriptionTextBox.Text = @"Launch Options:

Adds the following launch options to CSGO:

-console (Activates console ingame)
-freq XXX (The optimiser will automatically detect your monitors hertz limits and apply the best value)
-novid (Removes the short video on CSGO startup)
+exec autoexec.cfg (Executes the configuration file with optimised rates)
-high (Sets the csgo.exe process to 'high' priority for a small fps boost)";
            }
            else if (sender.Equals(mouseAccCheckBox))
            {
                descriptionTextBox.Text = @"Disable Mouse Acceleration:

The optimiser will automatically detect your setup and apply the correct 'MarkC No Acceleration' fix to windows registry for a perfect 1-to-1, no acceleration mouse input.

Recommended to also have 'Ingame Mouse Commands' applied as well.

(Credit to MarkC for his work with acceleration fixes)";

            }
            else if (sender.Equals(ingameMouseAccCheckBox))
            {
                descriptionTextBox.Text = @"Ingame Mouse commands:

Adds an IngameMouseAccelOff.cfg with the following commands:

m_forward 1
m_mousespeed 1
m_mouseaccel2 0
m_mouseaccel1 0
m_customaccel 0
m_customaccel_max 0
m_customaccel_exponent 0
m_customaccel_scale 0
m_rawinput 0";
            }
            else if (sender.Equals(capsLockCheckBox))
            {
                descriptionTextBox.Text = @"Disable Caps Lock for use with 'Push-To-Talk' hotkeys:

Disables the normal Caps Lock function (Key is remapped to F13) so you can use Caps Lock for Push-to-talk without accidently talking in CAPS.";
            }
            else if (sender.Equals(visualThemesCheckBox))
            {
                descriptionTextBox.Text = @"Deactivate visual themes on csgo.exe:

Deactivates Windows visuals on csgo.exe for a small fps boost.";
            }
            else if (sender.Equals(playersComboBox))
            {
                descriptionTextBox.Text = @"Select player profile:

Select a player profile to copy settings from. Player profiles are stored in \Resources\Players\.";
            }
        }

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            nvidiaProfileCheckBox.IsChecked = true;
            autoexecCheckBox.IsChecked = true;
            videoSettingsCheckBox.IsChecked = true;
            launchOptionsCheckBox.IsChecked = true;
            mouseAccCheckBox.IsChecked = true;
            ingameMouseAccCheckBox.IsChecked = true;
            capsLockCheckBox.IsChecked = true;
            visualThemesCheckBox.IsChecked = true;
        }

        private void deselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            nvidiaProfileCheckBox.IsChecked = false;
            autoexecCheckBox.IsChecked = false;
            videoSettingsCheckBox.IsChecked = false;
            launchOptionsCheckBox.IsChecked = false;
            mouseAccCheckBox.IsChecked = false;
            ingameMouseAccCheckBox.IsChecked = false;
            capsLockCheckBox.IsChecked = false;
            visualThemesCheckBox.IsChecked = false;
        }

        private void validateSteamPath()
        {
            if (optimiseController.GetSteamPath() == null)
            {
                throw new Exception("Please locate your Steam folder.");
            }
        }

        private void playersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IPlayer player = (IPlayer) playersComboBox.SelectedItem;
            if (player.Config != "")
            {
                configCheckBox.IsChecked = false;
                configCheckBox.IsEnabled = true;
            }
            else
            {
                configCheckBox.IsChecked = false;
                configCheckBox.IsEnabled = false;
            }
            if (player.Crosshair != "")
            {
                crosshairCheckBox.IsChecked = true;
                crosshairCheckBox.IsEnabled = true;
            }
            else
            {
                crosshairCheckBox.IsChecked = false;
                crosshairCheckBox.IsEnabled = false;
            }
            if (player.Autoexec != "")
            {
                autoexecCheckBox.IsChecked = true;
                autoexecCheckBox.IsEnabled = true;
            }
            else
            {
                autoexecCheckBox.IsChecked = false;
                autoexecCheckBox.IsEnabled = false;
            }
            if (player.VideoSettings != "")
            {
                videoSettingsCheckBox.IsChecked = true;
                videoSettingsCheckBox.IsEnabled = true;
            }
            else
            {
                videoSettingsCheckBox.IsChecked = false;
                videoSettingsCheckBox.IsEnabled = false;
            }
            if (player.LaunchOptions != "")
            {
                launchOptionsCheckBox.IsChecked = true;
                launchOptionsCheckBox.IsEnabled = true;
            }
            else
            {
                launchOptionsCheckBox.IsChecked = false;
                launchOptionsCheckBox.IsEnabled = false;
            }
            if (player.NvidiaProfile != "")
            {
                nvidiaProfileCheckBox.IsChecked = true;
                nvidiaProfileCheckBox.IsEnabled = true;
            }
            else
            {
                nvidiaProfileCheckBox.IsChecked = false;
                nvidiaProfileCheckBox.IsEnabled = false;
            }
            mouseAccCheckBox.IsChecked = player.DisabledMouseAcc;
            ingameMouseAccCheckBox.IsChecked = player.DisabledIngameMouseAcc;
            capsLockCheckBox.IsChecked = player.DisabledCapsLock;
            visualThemesCheckBox.IsChecked = player.DisabledVisualThemes;
        }
    }
}
