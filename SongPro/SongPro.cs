using System.Text.RegularExpressions;

namespace SongPro
{
    public static class SongPro
    {
        private static readonly Regex AttributeRegex = new Regex("@(\\w*)=([^%]*)");
        private static readonly Regex CustomAttributeRegex = new Regex("!(\\w*)=([^%]*)");
        private static readonly Regex SectionRegex = new Regex("#\\s*([^$]*)");

        private static readonly Regex ChordsAndLyricsRegex =
            new Regex("(\\[[\\w#b+/]+\\])?([^\\[]*)", RegexOptions.IgnoreCase);

        public static Song Parse(string lines)
        {
            var song = new Song();

            Section currentSection = null;

            foreach (var text in lines.Split('\n'))
            {
                if (text.StartsWith("@"))
                {
                    ProcessAttribute(song, text);
                }
                else if (text.StartsWith("!"))
                {
                    ProcessCustomAttribute(song, text);
                }
                else if (text.StartsWith("#"))
                {
                    currentSection = ProcessSection(song, text);
                }
                else
                {
                    ProcessLyricsAndChords(song, currentSection, text);
                }
            }

            return song;
        }

        private static void ProcessLyricsAndChords(Song song, Section currentSection, string text)
        {
            if (text.Equals(""))
            {
                return;
            }

            if (currentSection == null)
            {
                currentSection = new Section("");
                song.Sections.Add(currentSection);
            }

            var line = new Line();

            var match = ChordsAndLyricsRegex.Match(text);

            while (match.Success)
            {
                var part = new Part();

                if (match.Groups[1].Success)
                {
                    part.Chord = match.Groups[1].ToString().Trim().Replace("[", "").Replace("]", "");
                }
                else
                {
                    part.Chord = "";
                }

                if (match.Groups[2].Value != "")
                {
                    part.Lyric = match.Groups[2].Value;
                }
                else
                {
                    part.Lyric = "";
                }

                if (!(part.Chord == "" && part.Lyric == ""))
                {
                    line.Parts.Add(part);
                }

                match = match.NextMatch();
            }

            currentSection.Lines.Add(line);
        }

        private static Section ProcessSection(Song song, string text)
        {
            var match = SectionRegex.Match(text);
            if (match.Success)
            {
                var sectionName = match.Groups[1].Value;
                var section = new Section(sectionName);
                song.Sections.Add(section);
                return section;
            }

            return null;
        }

        private static void ProcessAttribute(Song song, string text)
        {
            var match = AttributeRegex.Match(text);
            if (match.Success)
            {
                var attributeName = match.Groups[1].Value;
                var attributeValue = match.Groups[2].Value;

                switch (attributeName)
                {
                    case "title":
                        song.Title = attributeValue;
                        break;
                    case "artist":
                        song.Artist = attributeValue;
                        break;
                    case "capo":
                        song.Capo = attributeValue;
                        break;
                    case "key":
                        song.Key = attributeValue;
                        break;
                    case "tempo":
                        song.Tempo = attributeValue;
                        break;
                    case "year":
                        song.Year = attributeValue;
                        break;
                    case "album":
                        song.Album = attributeValue;
                        break;
                    case "tuning":
                        song.Tuning = attributeValue;
                        break;
                }
            }
        }

        private static void ProcessCustomAttribute(Song song, string line)
        {
            var match = CustomAttributeRegex.Match(line);
            if (match.Success)
            {
                song.Custom[match.Groups[1].Value] = match.Groups[2].Value;
            }
        }
    }
}
