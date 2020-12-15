using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using xNet;

namespace MusicCrawl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private bool isCheckVN; public bool IsCheckVN { get => isCheckVN; set { isCheckVN = value; lsbTopSong.ItemsSource = ListVN; isCheckUSUK = false; isCheckCN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckUSUK"); OnPropertyChanged("IsCheckKR"); } }
        private bool isCheckUSUK; public bool IsCheckUSUK { get => isCheckUSUK; set { isCheckUSUK = value; lsbTopSong.ItemsSource = ListUSUK; isCheckVN = false; isCheckCN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckUSUK"); OnPropertyChanged("IsCheckKR"); } }
        private bool isCheckCN; public bool IsCheckCN { get => isCheckCN; set { isCheckCN = value; isCheckVN = false; lsbTopSong.ItemsSource = listCN; isCheckUSUK = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckUSUK"); OnPropertyChanged("IsCheckKR"); } }
        private ObservableCollection<Song> listVN; public ObservableCollection<Song> ListVN { get => listVN; set => listVN = value; }
        private ObservableCollection<Song> listUSUK; public ObservableCollection<Song> ListUSUK { get => listUSUK; set => listUSUK = value; }

        private ObservableCollection<Song> listCN; public ObservableCollection<Song> ListCN { get => listCN; set => listCN = value; }
        private Song currentSong; public Song CurrentSong { get => currentSong; set=> currentSong = value; }


        public MainWindow()
        {
            InitializeComponent();
            LoadingWindow loading = new LoadingWindow();
            this.Hide();
            loading.Show();
            
            ucSongInfo.BackToMain += UcSongInfo_BackToMain;
            this.DataContext = this;
            ListVN = new ObservableCollection<Song>();
            ListUSUK = new ObservableCollection<Song>();
            ListCN = new ObservableCollection<Song>();
            if (CrawlBXH())
            {
                loading.Close();
                this.Show();
            }

            IsCheckVN = true;
        }

        private void UcSongInfo_BackToMain(object sender, EventArgs e)
        {
            grdTop20.Visibility = Visibility.Visible;
            ucSongInfo.Visibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Song song = (sender as Button).DataContext as Song;
            CurrentSong = song;
            grdTop20.Visibility = Visibility.Hidden;
            ucSongInfo.Visibility = Visibility.Visible;
            ucSongInfo.SongInfo = song;
        }
        bool CrawlBXH()
        {
            addToList(ListVN, "vietnam");
            addToList(ListUSUK, "us-uk");
            addToList(ListCN, "chinese");
            return true;
        }
        void addToList(ObservableCollection<Song> listSong, string strList)
        {
            HttpRequest http = new HttpRequest();
            for (int i = 1; i <= 20; i++)
            {
                string link = "https://chiasenhac.vn/nhac-hot/" + strList + ".html?playlist=" + i.ToString();
                string htmlBXH = http.Get(link).ToString();
                string htmlPattern = @"<h1 class=""title"">(.*?)</h1>";
                var temp = Regex.Match(htmlBXH, htmlPattern, RegexOptions.Singleline);
                string songName = temp.ToString().Substring(">", " -");
                string singerName = temp.ToString().Substring("- ", "</");
                string urlPattern = "<link rel=\"canonical\"(.*?)>";
                var temp2 = Regex.Matches(htmlBXH, urlPattern, RegexOptions.Singleline);
                string songURL = temp2[0].ToString();
                songURL = songURL.Substring("https://", "\"");
                songURL = "https://" + songURL;
                string lyric = "Chưa có lời bài hát";
                string lyricPattern = "<div id=\"fulllyric\"(.*?)</div>";
                var getLyric = Regex.Matches(htmlBXH, lyricPattern, RegexOptions.Singleline);
                if (getLyric.Count > 0)
                {
                    lyric = getLyric[0].ToString();
                    lyric = lyric.Replace("<div id=\"fulllyric\">", "");
                    lyric = lyric.Replace("</div>", "");
                    lyric = lyric.Replace("<br />", "");
                    lyric = lyric.Trim();
                }
                string htmlSong = http.Get(songURL).ToString();
                var getLinkDownload = Regex.Match(htmlSong, "<li><a class=\"download_item\" href=\"https://(.*?)m4a\"", RegexOptions.Singleline);
                string linkDownload = getLinkDownload.ToString();
                linkDownload = linkDownload.Substring("https://", "\"");
                linkDownload = "https://" + linkDownload;
                var getLinkImage = Regex.Match(htmlSong, "<div id=\"companion_cover\">(.*?)</div>", RegexOptions.Singleline);
                string linkImage = getLinkImage.ToString();
                linkImage = linkImage.Substring("https://", "\"");
                linkImage = "https://" + linkImage; 
                string savePathImage = AppDomain.CurrentDomain.BaseDirectory + "Image\\" + songName + ".jpg";
                string savePath = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + songName + ".m4v";
                listSong.Add(new Song { SongName = songName, SingerName = singerName, SavePathImage = savePathImage ,Lyric = lyric, SongURL = songURL, STT = i.ToString(), DownloadURL = linkDownload,LinkImage = linkImage, SavePath = savePath });
            }
        }
        private void ChangeToNextSong(ObservableCollection<Song> listSong, int position, int addCount)
        {
            int index = listSong.IndexOf(CurrentSong);
            if (index == position)
            {
                return;
            }
            else
            {
                CurrentSong = listSong[index + addCount];
                ucSongInfo.SongInfo = CurrentSong;
            }
        }

        private void ucSongInfo_NextClicked(object sender, EventArgs e)
        {
            if (isCheckVN)
            {
                ChangeToNextSong(ListVN, 9, 1);
            }
            if (isCheckUSUK)
            {
                ChangeToNextSong(ListUSUK, 9, 1);
            }
            if (isCheckCN)
            {
                ChangeToNextSong(ListCN, 9, 1);
            }
        }

        private void ucSongInfo_PreviourClicked(object sender, EventArgs e)
        {
            if (isCheckVN)
            {
                ChangeToNextSong(ListVN, 0, -1);
            }
            else if (isCheckUSUK)
            {
                ChangeToNextSong(ListUSUK, 0, -1);
            }
            else
            {
                ChangeToNextSong(ListCN, 0, -1);
            }
        }
    }
}
