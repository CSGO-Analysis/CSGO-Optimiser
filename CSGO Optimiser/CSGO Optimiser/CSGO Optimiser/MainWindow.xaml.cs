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
        public static SteamBrowse steamBrowse;
        public MainWindow()
        {
            InitializeComponent();
            steamBrowse = new SteamBrowse();
            optimisation.Content = new Optimisation(steamBrowse);
            backup.Content = new Backup(steamBrowse);
        }
    }
}
