using OsuTools.Models.Beatmaps;
using System.Collections.Generic;

namespace OsuTools.Models.Database {
    public interface IDatabase {

        // Beatmaps that contains the Database
        IEnumerable<Beatmap> Beatmaps { get; set; }
    }
}
