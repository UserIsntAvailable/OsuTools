using System;
using static OsuTools.Utils.ScoreHelper;

namespace OsuTools.Models.Scores {
    public interface IScore {

        Ruleset Ruleset { get; set; }
        string BeatmapHash { get; set; }
        string Player { get; set; }
        string Hash { get; set; }
        Hits Hits { get; set; }
        int ScoreObtained { get; set; }
        short? MaxCombo { get; set; }
        bool PerfectCombo { get; set; }
        string ModsUsed { get; set; }
        long Timestand { get; set; }
        long OnlineScoreID { get; set; }

        double Accuracy { get { return GetAcc(Hits); } }
        DateTime Date { get { return GetDate((ulong)Timestand); } }
    }
}
