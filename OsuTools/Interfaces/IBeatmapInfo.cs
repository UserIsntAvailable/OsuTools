﻿using OsuTools.Models;

namespace OsuTools.Interfaces {
    public interface IBeatmapInfo {

        // Change all the ToString() Methods

        // Create an IScoreInfo

        Ruleset Mode { get; set; }
        string Hash { get; set; }
        int MaxCombo { get; set; }
    }
}
