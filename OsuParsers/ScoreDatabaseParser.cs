using System.IO;
using System.Linq;
using OsuTools.Utils;
using OsuTools.Models;

namespace OsuParsers {
    public static class ScoreDatabaseParser {

        #region Private Property

        private static BinaryReader Reader { get; set; }
        #endregion
        
        /// <summary>
        /// Parse a Score.db
        /// </summary>
        /// <param name="path">The path of your score.db</param>
        /// <returns>The ScoreDatabase</returns>
        public static ScoreDatabase Parse(string path) {

            Reader = new BinaryReader(File.OpenRead(path));

            // (Int) Version of the .db (e.g 20150204)
            Reader.ReadBytes(4);

            ScoreDatabase scoreDatabase = new ScoreDatabase {

                // Reader.ReadInt32() = The total number of Beatmaps in the ScoreDatabase
                Beatmaps = from _ in Enumerable.Range(0, Reader.ReadInt32()) select GetBeatmap()
            };

            return scoreDatabase;
        }

        #region Private Methods

        /// <summary>
        /// Get Beatmaps scores from the ScoreDatabase
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
        /// Get Score from the current Beatmap
        /// </summary>
        /// <returns>Scores of the current Beatmap</returns>
        private static Score GetScore()  {

            Score score = new Score();

            score.Mode = EnumHelper.GetEnumField<GameMode>(Reader.ReadByte());

            // Version of this score (e.g. 20150203)
            int version = Reader.ReadInt32();

            score.Hash = Reader.ReadOsuString();
            score.Player = Reader.ReadOsuString();
            score.HashScore = Reader.ReadOsuString();

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

            //String (Should always be empty)
            Reader.ReadOsuString();

            score.Timestand = Reader.ReadInt64();

            //Int (Should always be 0xffffffff (-1))
            Reader.ReadBytes(4);

            if (version >= 20140721) {
                score.OnlineScoreID = Reader.ReadInt64();
            }
            else if (version >= 20121008) {
                score.OnlineScoreID = Reader.ReadInt32();
            }

            Reader.Dispose();

            return score;
        }
        #endregion
    }
}
