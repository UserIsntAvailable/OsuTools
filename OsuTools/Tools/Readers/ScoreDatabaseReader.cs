using System.IO;
using System.Linq;
using OsuTools.Utils;
using OsuTools.Models.Beatmaps;
using OsuTools.Models.Database;
using OsuTools.Models.Scores;

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
                Beatmaps = from _ in Enumerable.Range(0, Reader.ReadInt32()) select ReadBeatmap()
            };

            return scoreDatabase;
        }

        /// <summary>
        /// Get Beatmaps scores from <see cref="ScoreDatabase">
        /// </summary>
        /// <returns>Beatmaps with his scores</returns>
        private static Beatmap ReadBeatmap() {

            // Beatmap Hash
            Reader.ReadOsuString();

            Beatmap beatmap = new Beatmap() {

                // Reader.ReadInt32() = Number of Scores mades in this Beatmap
                Scores = from _ in Enumerable.Range(0, Reader.ReadInt32()) select (Score)Reader.ReadScore(false)
            };

            return beatmap;
        }
    }
}
