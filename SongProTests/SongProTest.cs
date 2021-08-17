using NUnit.Framework;

namespace SongProTests
{
    using SongPro = SongPro.SongPro;

    [TestFixture]
    public class SongProTest
    {
        [Test]
        public void AttributeParsing()
        {
            var song = SongPro.Parse(@"
@title=Bad Moon Rising
@artist=Creedence Clearwater Revival
@capo=1st Fret
@key=C# Minor
@tempo=120
@year=1975
@album=Foo Bar Baz
@tuning=Eb Standard
");

            Assert.That(song.Title, Is.EqualTo("Bad Moon Rising"));
            Assert.That(song.Artist, Is.EqualTo("Creedence Clearwater Revival"));
            Assert.That(song.Capo, Is.EqualTo("1st Fret"));
            Assert.That(song.Key, Is.EqualTo("C# Minor"));
            Assert.That(song.Tempo, Is.EqualTo("120"));
            Assert.That(song.Year, Is.EqualTo("1975"));
            Assert.That(song.Album, Is.EqualTo("Foo Bar Baz"));
            Assert.That(song.Tuning, Is.EqualTo("Eb Standard"));
        }

        [Test]
        public void CustomAttributeParsing()
        {
            var song = SongPro.Parse(@"
!difficulty=Easy
!spotify_url=https://open.spotify.com/track/5zADxJhJEzuOstzcUtXlXv?si=SN6U1oveQ7KNfhtD2NHf9A
");

            Assert.That(song.Custom["difficulty"], Is.EqualTo("Easy"));
            Assert.That(song.Custom["spotify_url"],
                Is.EqualTo("https://open.spotify.com/track/5zADxJhJEzuOstzcUtXlXv?si=SN6U1oveQ7KNfhtD2NHf9A"));
        }

        [Test]
        public void ParseSectionNames()
        {
            var song = SongPro.Parse(@"
# Verse 1
# Chorus
");

            Assert.That(song.Sections.Count, Is.EqualTo(2));
            Assert.That(song.Sections[0].Name, Is.EqualTo("Verse 1"));
            Assert.That(song.Sections[1].Name, Is.EqualTo("Chorus"));
        }

        [Test]
        public void ParseLyrics()
        {
            var song = SongPro.Parse(@"I don't see! a bad, moon a-rising. (a-rising)");

            Assert.That(song.Sections.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines[0].Parts.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines[0].Parts[0].Lyric,
                Is.EqualTo("I don't see! a bad, moon a-rising. (a-rising)"));
        }

        [Test]
        public void ParseChords()
        {
            var song = SongPro.Parse($"[D] [D/F#] [C] [A7]");

            Assert.That(song.Sections.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines[0].Parts.Count, Is.EqualTo(4));
            Assert.That(song.Sections[0].Lines[0].Parts[0].Chord, Is.EqualTo("D"));
            Assert.That(song.Sections[0].Lines[0].Parts[0].Lyric, Is.EqualTo(" "));
            Assert.That(song.Sections[0].Lines[0].Parts[1].Chord, Is.EqualTo("D/F#"));
            Assert.That(song.Sections[0].Lines[0].Parts[1].Lyric, Is.EqualTo(" "));
            Assert.That(song.Sections[0].Lines[0].Parts[2].Chord, Is.EqualTo("C"));
            Assert.That(song.Sections[0].Lines[0].Parts[2].Lyric, Is.EqualTo(" "));
            Assert.That(song.Sections[0].Lines[0].Parts[3].Chord, Is.EqualTo("A7"));
            Assert.That(song.Sections[0].Lines[0].Parts[3].Lyric, Is.EqualTo(""));
        }

        [Test]
        public void ParseChordsAndLyrics()
        {
            var song = SongPro.Parse(@"It's a[D]bout a [E]boy");

            Assert.That(song.Sections.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines.Count, Is.EqualTo(1));
            Assert.That(song.Sections[0].Lines[0].Parts.Count, Is.EqualTo(3));
            Assert.That(song.Sections[0].Lines[0].Parts[0].Chord, Is.EqualTo(""));
            Assert.That(song.Sections[0].Lines[0].Parts[0].Lyric, Is.EqualTo("It's a"));
            Assert.That(song.Sections[0].Lines[0].Parts[1].Chord, Is.EqualTo("D"));
            Assert.That(song.Sections[0].Lines[0].Parts[1].Lyric, Is.EqualTo("bout a "));
            Assert.That(song.Sections[0].Lines[0].Parts[2].Chord, Is.EqualTo("E"));
            Assert.That(song.Sections[0].Lines[0].Parts[2].Lyric, Is.EqualTo("boy"));
        }
    }
}
