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
    /// Interaction logic for Optimisation.xaml
    /// </summary>
    public partial class Optimisation : UserControl
    {
        private OptimiseController optimiseController;
        private ProfileController profileController;
        private BackupController backupController;

        public Optimisation(BackupController bkController)
        {
            InitializeComponent();
            optimiseController = new OptimiseController();
            profileController = new ProfileController();
            backupController = bkController;
            profilesComboBox.ItemsSource = profileController.GetProfiles();
            foreach (IProfile p in profileController.GetProfiles())
            {
                if (p.Name == "Recommended")
                {
                    profilesComboBox.SelectedItem = p;
                    break;
                }
            }
        }

        private void optimiseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SteamController.ValidateSteamPath();
                int changes = 0;
                bool reboot = false;
                // Profile Settings:
                if (profilesComboBox.SelectedItem != null)
                {
                    if (backupController.GetBackups().Count <= 0)
                    {
                        if (MessageBox.Show("You do not have any saved backups. Do you want to save your current settings as backup?",
                                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            logTextBox.Text += backupController.SaveBackup();
                        }
                    }
                    IProfile profile = (IProfile) profilesComboBox.SelectedItem;
                    if (autoexecCheckBox.IsChecked == true) // Must be first because other methods write in this file.
                    {
                        logTextBox.Text += optimiseController.CopyAutoexec(profile);
                        changes++;
                    }
                    if (configCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.CopyProfileConfig(profile);
                        changes++;
                    }
                    if (crosshairCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.CopyProfileCrosshair(profile);
                        changes++;
                    }
                    if (videoSettingsCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.CopyVideoConfig(profile);
                        changes++;
                    }
                    if (launchOptionsCheckBox.IsChecked == true)
                    {
                        Process[] steamProcess = Process.GetProcessesByName("Steam");
                        if (steamProcess.Length != 0)
                        {
                            if (MessageBox.Show("Steam must be restarted in order to add launch options. Restart Steam?",
                                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                steamProcess[0].Kill();
                                steamProcess[0].WaitForExit();
                                logTextBox.Text += optimiseController.SetLaunchOptions(profile);
                                changes++;
                                Process.Start(SteamController.GetSteamPath() + "\\Steam.exe");
                            }
                            else
                            {
                                logTextBox.Text += "Launch options was not added. \n";
                            }
                        }
                        else
                        {
                            logTextBox.Text += optimiseController.SetLaunchOptions(profile);
                            changes++;
                        }
                    }
                    if (nvidiaProfileCheckBox.IsChecked == true)
                    {
                        logTextBox.Text += optimiseController.SetNvidiaSettings(profile);
                        changes++;
                    }
                }
                // Global settings:
                if (mouseAccCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableMouseAcc();
                    changes++;
                    reboot = true;
                }
                if (ingameMouseAccCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableIngameMouseAcc();
                    changes++;
                }
                if (capsLockCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableCapsLock();
                    changes++;
                    reboot = true;
                }
                if (visualThemesCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableVisualThemes();
                    changes++;
                    reboot = true;
                }
                logTextBox.Text += string.Format("Optimisation finished ({0} changes). \n", changes);
                if (reboot == true)
                {
                    logTextBox.Text += "Please reboot to apply the registry changes. \n";
                }
                logTextBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void profilesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IProfile profile = (IProfile) profilesComboBox.SelectedItem;
            deselectAllButton_Click(null, null);

            if (profile.Config != "")
                configCheckBox.IsEnabled = true;
            else
                configCheckBox.IsEnabled = false;

            if (profile.Crosshair != "")
                crosshairCheckBox.IsEnabled = true;
            else
                crosshairCheckBox.IsEnabled = false;

            if (profile.Autoexec != "")
                autoexecCheckBox.IsEnabled = true;
            else
                autoexecCheckBox.IsEnabled = false;

            if (profile.VideoSettings != "")
                videoSettingsCheckBox.IsEnabled = true;
            else
                videoSettingsCheckBox.IsEnabled = false;

            if (profile.LaunchOptions != "")
                launchOptionsCheckBox.IsEnabled = true;
            else
                launchOptionsCheckBox.IsEnabled = false;

            if (profile.NvidiaProfile != "")
                nvidiaProfileCheckBox.IsEnabled = true;
            else
                nvidiaProfileCheckBox.IsEnabled = false;

            //mouseAccCheckBox.IsChecked = profile.DisabledMouseAcc;
            //ingameMouseAccCheckBox.IsChecked = profile.DisabledIngameMouseAcc;
            //capsLockCheckBox.IsChecked = profile.DisabledCapsLock;
            //visualThemesCheckBox.IsChecked = profile.DisabledVisualThemes;
        }

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Control c in checkBoxStackPanel.Children.OfType<CheckBox>())
            {
                if (c.GetType() == typeof(CheckBox) && c.IsEnabled == true)
                    ((CheckBox)c).IsChecked = true;
            }
        }

        private void deselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Control c in checkBoxStackPanel.Children.OfType<CheckBox>())
            {
                if (c.GetType() == typeof(CheckBox))
                    ((CheckBox)c).IsChecked = false;
            }
        }

        // Should probably be moved to Resources..
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (sender.Equals(profilesComboBox))
            {
                descriptionTextBox.Text =
@"Select player profile:

Select a player profile to copy settings from. Player profiles are stored in \Resources\Profiles\.";
            }
            else if (sender.Equals(configCheckBox))
            {
                descriptionTextBox.Text =
@"Config (not recommended):

Copies the selected profile's config.cfg to your csgo folder. This will overwrite all of your keybinds and settings.";
            }
            else if (sender.Equals(crosshairCheckBox))
            {
                descriptionTextBox.Text =
@"Crosshair Settings:

Copies the selected profile's crosshair settings to a new cfg, executed from autoexec.cfg.";
            }
            else if (sender.Equals(autoexecCheckBox))
            {
                descriptionTextBox.Text =
@"Autoexec:

Copies the selected profile's autoexec.cfg to your csgo folder.";
            }
            else if (sender.Equals(videoSettingsCheckBox))
            {
                descriptionTextBox.Text =
@"Advanced Ingame Video Settings:

Copies the selected profile's ingame video settings (video.txt) to your csgo folder.

(Highly recommended to also use the NVIDIA CSGO Profile to avoid settings being overruled)";
            }
            else if (sender.Equals(launchOptionsCheckBox))
            {
                descriptionTextBox.Text =
@"Launch Options:

Copies the selected profile's CSGO launch options.

Most common launch options are:

-console (Activates console ingame)
-novid (Removes the short video on CSGO startup)
+exec autoexec.cfg (Executes the configuration file with optimised rates)
-high (Sets the csgo.exe process to 'high' priority for a small fps boost)";
            }
            else if (sender.Equals(nvidiaProfileCheckBox))
            {
                descriptionTextBox.Text =
@"NVIDIA CSGO Profile:

Uses nvidiaInspector to import the selected profile's NVIDIA 3D profile that changes the driver settings for csgo.exe only.";
            }
            else if (sender.Equals(mouseAccCheckBox))
            {
                descriptionTextBox.Text =
@"Disable Mouse Acceleration:

The optimiser will automatically detect your setup and apply the correct 'MarkC No Acceleration' fix to windows registry for a perfect 1-to-1, no acceleration mouse input.

Recommended to also have 'Disable Ingame Mouse Acceleration' applied as well.

(Credit to MarkC for his work with acceleration fixes)";
            }
            else if (sender.Equals(ingameMouseAccCheckBox))
            {
                descriptionTextBox.Text =
@"Ingame Mouse Commands:

Adds an IngameMouseAccelOff.cfg (executed from autoexec) with the following commands:
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
                descriptionTextBox.Text =
@"Disable Caps Lock for use with 'Push-To-Talk' hotkeys:

Disables the normal Caps Lock function (Key is remapped to F13) so you can use Caps Lock for Push-to-talk without accidently talking in CAPS.";
            }
            else if (sender.Equals(visualThemesCheckBox))
            {
                descriptionTextBox.Text =
@"Deactivate visual themes on csgo.exe:

Deactivates Windows visuals on csgo.exe for a small fps boost.";
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