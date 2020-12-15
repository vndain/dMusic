using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicCrawl.dUserControl
{
    /// <summary>
    /// Interaction logic for ucControlBar.xaml
    /// </summary>
    public partial class ucControlBar : UserControl
    {
        public ControlBarUC Viewmodel { get; set; }
        public ucControlBar()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ControlBarUC();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggle = new ToggleButton();
            if (toggle.IsChecked == true)
            {

            }
        }
    }
}
