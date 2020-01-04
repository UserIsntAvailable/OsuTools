using Xunit;
using System;
using OsuTools.Models;
using OsuTools.Models.Scores;
using static OsuTools.Tests.Resources.ResourcesHelper;

namespace OsuTools.Tests.Tools.Readers {
    public class ReplayReaderTests {

        private string filepath;

        [Fact]
        public void ReadingASingleFileTest() {

            filepath = GetResourcePathFile("replay-osu_1206116.osr");

            Replay replay = Replay.Read(filepath);

            Assert.Equal(Ruleset.Standard, replay.Ruleset);
            Assert.Equal("ddacbb2305f94dc0998bb85837c569f1", replay.BeatmapHash);
            Assert.Equal("ANDavid1611", replay.Player);
            // Hash: IDK it is posible to calculate the Hash of a .osr file
            Assert.Equal(new Hits(298, 2, 0, 73, 1, 0), replay.Hits);
            Assert.Equal(4262174, replay.ScoreObtained);
            Assert.Equal((short)482, replay.MaxCombo);
            Assert.True(replay.PerfectCombo);
            Assert.Equal("HDDT", replay.ModsUsed);
            Assert.Equal(2870143131, replay.OnlineScoreID);
            Assert.Equal(new DateTime(2019, 8, 10, 23, 42, 56), replay.Date);
            Assert.Equal(99.56, replay.Accuracy);
            // ScoreGraphBar: No idea how test it
            // Frames: I will test this later with the OsuFileReader, I'm just too lazy now :p
        }
    }
}
