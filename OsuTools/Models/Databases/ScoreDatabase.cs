using System.Collections.Generic;

namespace OsuTools.Models {
    public class ScoreDatabase : BaseDatabase {

        internal override IEnumerable<Beatmap> Beatmaps { get; set; }
    }
}