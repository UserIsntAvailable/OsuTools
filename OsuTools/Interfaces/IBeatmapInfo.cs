using OsuTools.Models;

namespace OsuTools.Interfaces {
    public interface IBeatmapInfo {

        // Change all the ToString() Methods

        // Create an IScoreInfo

        GameMode Mode { get; set; }
        string Hash { get; set; }
        int MaxCombo { get; set; }
    }
}
