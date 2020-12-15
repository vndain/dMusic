using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;

namespace MusicCrawl.dUserControl
{
    /// <summary>
    /// Interaction logic for SongInfoUC.xaml
    /// </summary>
    public partial class SongInfoUC : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        } 
        private bool isPlaying; 
        public bool IsPlaying 
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                if (isPlaying)
                {
                    mdAudio.Play();
                    iconPlay.Kind = PackIconKind.Pause;
                    timer.Start();
                }
                else
                {
                    mdAudio.Pause();
                    iconPlay.Kind = PackIconKind.Play;
                    timer.Stop();
                }
            }
        }
        DispatcherTimer timer;
        public SongInfoUC()
        {
            InitializeComponent();
            this.DataContext = SongInfo;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SongInfo.Position ++;
            sdDuration.Value = SongInfo.Position;
        }

        private void mdAudio_MediaOpened(object sender, RoutedEventArgs e)
        {            
            SongInfo.Duration = mdAudio.NaturalDuration.TimeSpan.TotalSeconds;
            txbDuration.Text = new TimeSpan(0, (int)SongInfo.Duration / 60, (int)SongInfo.Duration % 60).ToString("mm\\:ss");
            IsPlaying = true;
            sdDuration.Maximum = songInfo.Duration;
            SongInfo.Position = 0;
            timer.Start();
        }
        private Song songInfo; public Song SongInfo { get => songInfo; set { songInfo = value; DownloadSongImage(songInfo); this.DataContext = SongInfo; OnPropertyChanged("SongInfo"); } }
        void DownloadSongImage(Song songInfo)
        {
            string songName = songInfo.SavePath;
            string songNameImage = songInfo.SavePathImage;
            WebClient wc = new WebClient();
            if (!File.Exists(songName))
            {                
                wc.DownloadFile(songInfo.DownloadURL, songName);
            }
            if (!File.Exists(songNameImage))
            {
                wc.DownloadFile(songInfo.LinkImage, songNameImage);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (backToMain != null)
            {
                backToMain(this, new EventArgs());
            }
        }
        private event EventHandler previourClicked; 
        public event EventHandler PreviourClicked
        {
            add { previourClicked += value; }
            remove { previourClicked -= value; }
        }
        private event EventHandler nextClicked;
        public event EventHandler NextClicked
        {
            add { nextClicked += value; }
            remove { nextClicked -= value; }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (nextClicked != null)
            {
                nextClicked(this, new EventArgs());
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (previourClicked != null)
            {
                previourClicked(this, new EventArgs());
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            IsPlaying = !IsPlaying;
        }
        bool isDraging = false;
        private void sdDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDraging)
            {
                SongInfo.Position = sdDuration.Value;
                mdAudio.Position = new TimeSpan(0, 0, (int)SongInfo.Position);
            }
            txbPosition.Text = new TimeSpan(0, (int)SongInfo.Position / 60, (int)SongInfo.Position % 60).ToString("mm\\:ss");
        }

        private void sdDuration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDraging = true;
        }

        private void sdDuration_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDraging = false;
        }


  
    }
}
