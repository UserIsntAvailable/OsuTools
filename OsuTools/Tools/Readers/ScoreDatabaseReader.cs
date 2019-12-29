using System.IO;
using System.Linq;
using OsuTools.Utils;
using OsuTools.Models;
using OsuTools.Models.Beatmaps;
using OsuTools.Models.Database;

namespace OsuTools.Tools.Readers {
    public static class ScoreDatabaseReader {

        /// <summary>
        /// Reader used from Parse.
        /// </summary>
        private static BinaryReader Reader;

        /// <summary>
        /// Parse a score.db
        /// </summary>
        /// <param name="path">The path of your score.db</param>
        /// <returns><see cref="ScoreDatabase"></returns>
        public static ScoreDatabase Read(string path) {

            Reader = new BinaryReader(File.OpenRead(path));

            ScoreDatabase scoreDatabase = new ScoreDatabase {

                Version = Reader.ReadInt32(),

                // Reader.ReadInt32() = The total number of Beatmaps in the score.db
                Beatmaps = from _ in Enumerable.Range(0, Reader.ReadInt32()) select GetBeatmap()
            };

            return scoreDatabase;
        }

        #region Private Methods

        /// <summary>
        /// Get Beatmaps scores from <see cref="ScoreDatabase">
        /// </summary>
        /// <returns>Beatmaps with his scores</returns>
        private static Beatmap GetBeatmap() {

            // Beatmap Hash
            Reader.ReadOsuString();

            Beatmap beatmap = new Beatmap() {

                // Reader.ReadInt32() = Number of Scores mades in this Beatmap
                Scores = from _ in Enumerable.Range(0, Reader.ReadInt32()) select GetScore()
            };

            return beatmap;
        }

        /// <summary>
        /// Get Score from <see cref="Beatmap">
        /// </summary>
        /// <returns>Scores of the current <see cref="Beatmap"></returns>
        private static Score GetScore() {

            Score score = new Score();

            score.Ruleset = EnumHelper.GetEnumField<Ruleset>(Reader.ReadByte());

            // Version of this score (e.g. 20150203)
            int version = Reader.ReadInt32();

            score.BeatmapHash = Reader.ReadOsuString();
            score.Player = Reader.ReadOsuString();
            score.Hash = Reader.ReadOsuString();

            #region Hits parser

            Hits hits = new Hits();

            var hitsprops = hits.GetType().GetProperties();

            for (int i = 0; i < hitsprops.Length; i++) {

                hitsprops[i].SetValue(hits, Reader.ReadInt16());
            }

            score.Hits = hits;
            #endregion

            score.ScoreObtained = Reader.ReadInt32();
            score.MaxCombo = Reader.ReadInt16();
            score.PerfectCombo = Reader.ReadBoolean();
            score.ModsUsed = Reader.ReadInt32().ToString();

            // Should always be empty
            Reader.ReadOsuString();

            score.Timestand = Reader.ReadInt64();

            // Int (Should always be 0xffffffff [-1])
            Reader.ReadBytes(4);

            if (version >= 20140721) {
                score.OnlineScoreID = Reader.ReadInt64();
            }
            else if (version >= 20121008) {
                score.OnlineScoreID = Reader.ReadInt32();
            }

            return score;
        }
        #endregion
    }
}
