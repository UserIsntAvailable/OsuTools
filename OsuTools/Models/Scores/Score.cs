using System;
using OsuTools.Models.Enums;
using static OsuTools.Utils.ScoreHelper;

namespace OsuTools.Models.Scores {
    public class Score : IScore {

        #region Constructor

        public Score() { }

        public override string ToString() {
            return $"{Player} - {Ruleset} - {ModsUsed} - {Accuracy}";
        }
        #endregion

        #region Public Properties

        public Ruleset Ruleset { get; set; }
        public string BeatmapHash { get; set; }
        public string Player { get; set; }
        public string Hash { get; set; }
        public Hits Hits { get; set; }
        public int ScoreObtained { get; set; }
        public short? MaxCombo { get; set; }
        public bool PerfectCombo { get; set; }
        public string ModsUsed {
            get { return modsUsed; }
            set { modsUsed = BitwiseToString(long.Parse(value)); }
        }
        public long Timestand { get; set; }
        public long OnlineScoreID { get; set; }

        public double Accuracy => GetAcc(Hits);
        public DateTime Date => GetDate((ulong)Timestand);
        #endregion

        #region Private Field

        private string modsUsed;
        #endregion
    }
}