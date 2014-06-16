using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CSGO_Optimiser.UserControls
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About(Version curVersion)
        {
            InitializeComponent();
            versionTextBlock.Text = curVersion.ToString();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
