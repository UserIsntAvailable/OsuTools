using OsuTools.Models;

namespace OsuTools.Models.Beatmaps {
    public interface IBeatmapInfo {

        // Change all the ToString() Methods

        // Create an IScoreInfo

        Ruleset Ruleset { get; set; }
        string Hash { get; set; }
        int? MaxCombo { get; set; }
    }
}
