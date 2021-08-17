using System.Collections.Generic;

namespace SongPro
{
    public class Section
    {
        public Section(string sectionName)
        {
            Name = sectionName;
        }

        public string Name { get; set; }
        public List<Line> Lines { get; set; } = new List<Line>();
    }
}
