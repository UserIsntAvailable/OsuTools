using System.Collections;
using System.Collections.Generic;
using OsuTools.Models.Scores;

namespace OsuTools.Models.Database {
    public class ScoreDatabase : IEnumerable<Score> {

        #region IEnumerable Interface

        public IEnumerator<Score> GetEnumerator() {

            foreach (var beatmap in Beatmaps) {

                foreach (var score in beatmap.Scores) {

                    yield return score;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Public Properties

        public int Version { get; set; }

        public (string Hash, Score[] Scores)[] Beatmaps { get; set; }
        #endregion
    }
}