using System.Collections.Generic;

namespace OsuTools.Models {
    public abstract class BaseDatabase {

        // Beatmaps that contains the Database
        abstract internal IEnumerable<Beatmap> Beatmaps { get; set; }
    }
}
