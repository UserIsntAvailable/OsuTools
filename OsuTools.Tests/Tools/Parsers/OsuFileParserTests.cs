using Xunit;
using OsuTools.Models;
using OsuTools.Exceptions;
using OsuTools.Tools.Parsers;
using static OsuTools.Tests.Resources.ResourcesHelper;

namespace OsuTools {

    public class OsuFileParserTests {

        private string filePath;

        [Fact]
        public void BeatmapVersionTooOldTest() {

            filePath = GetResourcePathFile("beatmap_version_3.osu");

            Assert.Throws<OsuFileVersionTooOldException>(() => OsuFileParser.Parse(filePath));
        }

        [Fact]
        public void BeatmapVersionBadFormatedTest() {

            filePath = GetResourcePathFile("beatmap_version_bad_formated.osu");

            Assert.Throws<OsuFileFormatException>(() => OsuFileParser.Parse(filePath));
        }

        [Fact]
        public void ParsingASingleFileTest() {

            filePath = GetResourcePathFile("Wakeshima Kanon - World's End, Girl's Rondo.osu");

            Beatmap beatmap = OsuFileParser.Parse(filePath);

            // General
            Assert.Equal(Ruleset.Standard, beatmap.Ruleset);

            // Metadata
            Assert.Equal("World's End, Girl's Rondo(Asterisk DnB Remix)", beatmap.Title);
            Assert.Equal("Wakeshima Kanon", beatmap.Artist);
            Assert.Equal("Meg", beatmap.Creator);
            Assert.Equal("We cry \"OPEN\"", beatmap.Version);
            Assert.Equal("selector spread WIXOSS", beatmap.Source);
            Assert.Equal("opening Asterisk_ DnB F/C Works3 image--", beatmap.Tags);
            Assert.Equal(734339, beatmap.BeatmapID);
            Assert.Equal(331499, beatmap.BeatmapSetID);

            // Difficulty
            Assert.Equal(6, beatmap.HPDrainRate);
            Assert.Equal(4, beatmap.CircleSize);
            Assert.Equal(8, beatmap.OverallDifficulty);
            Assert.Equal(9, beatmap.ApproachRate);
            Assert.Equal(1.9, beatmap.SliderMultiplier);
            Assert.Equal(2, beatmap.SliderTickRate);

            // TimingPoints
            Assert.Contains("20821,-133.333333333333,4,2,10,60,0,0", beatmap.TimingPoints);
            Assert.Equal("65221,-100,4,2,2,10,0,0", beatmap.TimingPoints[20]);

            // HitObjects
            Assert.Contains("148,20,20821,38,0,B|138:48|110:62|110:62|148:71|166:94|176:122|171:155|168:184|144:204|101:202|56:200|20:188|4:164|8:144|16:120|52:96|52:96|64:120|96:128|96:128|81:226,1,570.000021743775,6|6,0:1|0:0,0:0:0:0:", beatmap.HitObjects);
            Assert.Equal("96,120,58535,2,0,B|144:113|144:113|190:116,1,95,8|0,0:0|0:0,0:0:0:0:", beatmap.HitObjects[165]);
        }
    }
}
