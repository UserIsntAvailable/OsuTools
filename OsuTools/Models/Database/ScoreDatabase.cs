using OsuTools.Models.Beatmaps;
using System.Collections;
using System.Collections.Generic;

namespace OsuTools.Models.Database {
    public class ScoreDatabase : IDatabase {

        public IEnumerable<Beatmap> Beatmaps { get; set; }

        public IEnumerator GetEnumerator() {

            foreach (var beatmap in Beatmaps) {
                foreach (var score in beatmap.Scores) {
                    yield return score;
                }
            }
        }
    }
}