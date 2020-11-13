using Xunit;
using System;
using System.Linq;
using OsuTools.Tools.Readers;
using OsuTools.Models.Database;
using static OsuTools.Tests.Resources.ResourcesHelper;

#if TESTMODE
namespace OsuTools.Tests.Tools.Readers {
    public class OsuDatabaseReaderTests {

        private string filePath;

        [Fact]
        public void ReadingASingleFileTest() {

            filePath = GetResourcePathFile("osu!.db");

            OsuDatabase osuDatabase = OsuDatabaseReader.Read(filePath);

            // This is the number of beatmaps that must have this osu!.db
            Assert.Equal(23864, osuDatabase.Count());

            Assert.Equal(20191227, osuDatabase.Version);
        }

        [Fact]
        // This is a Local Test. My Songs folder size is 74.5GB so... I can't copy to Resources.
        // I will create an OsuDatabaseWriter in the future so I would create an osu!.db perzonalized.
		// This was write the 7/26/2020 DO A THEORY!!!
        public void OsuFileParserPlusOsuDatabaseParserTest() {

            filePath = GetResourcePathFile("osu!.db");

            var songsPath = @$"C:\Users\{Environment.UserName}\AppData\Local\osu!\Songs";

            OsuDatabase osuDatabase = OsuDatabaseReader.Read(filePath);

            foreach (var osuDatabaseBeatmap in osuDatabase) {

                // This is because I have the .osu file but on my database the beatmap is completely empty.
                if (osuDatabaseBeatmap.Title == "") { }

                else {

                    var osuFileBeatmap = OsuFileReader.Read($"{songsPath}\\{osuDatabaseBeatmap.FilePath}");
                    Assert.True(osuDatabaseBeatmap.Equals(osuFileBeatmap));
                }
            };
        }
    }
}
#endif