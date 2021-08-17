using System.Collections.Generic;

namespace SongPro
{
    public class Song
    {
        public Song()
        {
        }

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Capo { get; set; }
        public string Key { get; set; }
        public string Tempo { get; set; }
        public string Year { get; set; }
        public string Album { get; set; }
        public string Tuning { get; set; }

        public Dictionary<string, string> Custom { get; } = new Dictionary<string, string>();
        public List<Section> Sections { get; } = new List<Section>();
    }
}
