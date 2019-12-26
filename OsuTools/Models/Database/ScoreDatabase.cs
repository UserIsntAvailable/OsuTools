using OsuTools.Models.Beatmaps;
using System.Collections.Generic;

namespace OsuTools.Models.Database {
    public class ScoreDatabase : IDatabase {

        public IEnumerable<Beatmap> Beatmaps { get; set; }
    }
}