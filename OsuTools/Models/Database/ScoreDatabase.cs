using System.Collections;
using System.Collections.Generic;
using OsuTools.Models.Scores;
using OsuTools.Models.Beatmaps;

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

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion

        public int Version { get; set; }

        public IEnumerable<Beatmap> Beatmaps { get; set; }
    }
}