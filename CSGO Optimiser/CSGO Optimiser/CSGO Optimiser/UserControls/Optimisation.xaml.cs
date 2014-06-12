﻿using Common;
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

        public Optimisation()
        {
            InitializeComponent();
            optimiseController = new OptimiseController();
            profileController = new ProfileController();
            profilesComboBox.ItemsSource = profileController.GetProfiles();
            foreach (IProfile p in profileController.GetProfiles())
            {
                if (p.Name == "Default")
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
                validateSteamPath();
                int changes = 0;
                // Profile Settings:
                if (profilesComboBox.SelectedItem != null)
                {
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
                            if (MessageBox.Show("Steam must be closed in order to add launch options. Shutdown Steam?",
                                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                steamProcess[0].Kill();
                                steamProcess[0].WaitForExit();
                                logTextBox.Text += optimiseController.SetLaunchOptions(profile);
                                changes++;
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
                }
                if (visualThemesCheckBox.IsChecked == true)
                {
                    logTextBox.Text += optimiseController.DisableVisualThemes();
                    changes++;
                }
                logTextBox.Text += string.Format("Optimisation finished ({0} changes). \n", changes);
                logTextBox.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void validateSteamPath()
        {
            if (SteamController.GetSteamPath() == null)
            {
                throw new Exception("Please locate your Steam folder.");
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

            mouseAccCheckBox.IsChecked = profile.DisabledMouseAcc;
            ingameMouseAccCheckBox.IsChecked = profile.DisabledIngameMouseAcc;
            capsLockCheckBox.IsChecked = profile.DisabledCapsLock;
            visualThemesCheckBox.IsChecked = profile.DisabledVisualThemes;
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
                descriptionTextBox.Text = @"Select profile profile:

Select a profile profile to copy settings from. Profile profiles are stored in \Resources\Profiles\.";
            }
            else if (sender.Equals(configCheckBox))
            {
                descriptionTextBox.Text = @"lol hej";
            }
            else if (sender.Equals(crosshairCheckBox))
            {
                descriptionTextBox.Text = @"hæ hæ";
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
            else if (sender.Equals(nvidiaProfileCheckBox))
            {
                descriptionTextBox.Text = @"NVIDIA CSGO Profile:

Uses nvidiaInspector to import a NVIDIA 3D profile that changes the driver settings for csgo.exe only.";
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