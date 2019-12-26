using System;
using OsuTools.Interfaces;
using static OsuTools.Utils.ScoreHelper;

namespace OsuTools.Models {
    public class Score : IBeatmapInfo {

        #region Constructor
        /// <summary>
        /// Empty Constructor
        /// </summary>
        internal Score() { }

        public override string ToString() {
            return $"{Ruleset} - {Accuracy} - {ModsUsed} - {Player}";
        }
        #endregion

        #region Public Properties

        public Ruleset Ruleset { get; set; }
        public string Hash { get; set; }
        public int? MaxCombo { get; set; }

        public string Player { get; set; }
        public string HashScore { get; set; }
        public Hits Hits { get; set; }
        public int ScoreObtained { get; set; }
        public bool PerfectCombo { get; set; }
        public string ModsUsed { 
            get { return modsUsed; }
            set { modsUsed = BitwiseToString(long.Parse(value)); }}
        public long Timestand { get; set; }
        public long OnlineScoreID { get; set; }

        public double Accuracy { get { return GetAcc(Hits); } }
        public DateTime Date { get { return GetDate((ulong)Timestand); } }
        #endregion

        #region Private Property

        private string modsUsed;
        #endregion
    }
}