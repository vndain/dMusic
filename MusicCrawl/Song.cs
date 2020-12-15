using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCrawl
{
    public class Song
    {
        private string songName; public string SongName { get => songName; set => songName = value; }
        private string singerName; public string SingerName { get => singerName; set => singerName = value; }
        private string songURL; public string SongURL { get => songURL; set => songURL = value; }
        private string sTT; public string STT { get => sTT; set => sTT = value; }
        private string lyric; public string Lyric { get => lyric; set => lyric = value; }
        private string downloadURL; public string DownloadURL { get => downloadURL; set => downloadURL = value; }
        private string savePath; public string SavePath { get => savePath; set => savePath = value; }
        private double duration; public double Duration { get => duration; set => duration = value; }
        private double position; public double Position { get => position; set => position = value; }
        private string linkImage; public string LinkImage { get => linkImage; set => linkImage = value; }
        private string savePathImage; public string SavePathImage { get => savePathImage; set => savePathImage = value; }

    }
}
