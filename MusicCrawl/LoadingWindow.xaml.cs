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
using System.Windows.Shapes;

namespace MusicCrawl
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private bool loadedData; public bool LoadedData { get => loadedData; set => loadedData = value; }
        public LoadingWindow()
        {
            InitializeComponent();
            if (LoadedData == true)
            {
                MainWindow main = new MainWindow();
                this.Hide();
                main.ShowDialog();
                this.Close();
            }
        }
    }
}
