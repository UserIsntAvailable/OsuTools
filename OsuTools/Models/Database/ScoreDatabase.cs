using OsuTools.Models.Beatmaps;
using System.Collections;
using System.Collections.Generic;

namespace OsuTools.Models.Database {
    public class ScoreDatabase : IEnumerable<Score> {

        public int Version { get; set; }

        public IEnumerable<Beatmap> Beatmaps { get; set; }

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
    }
}