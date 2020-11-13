using System;
using System.IO;
using OsuTools.Utils;
using OsuTools.Models.Enums;
using static OsuTools.Utils.ScoreHelper;

namespace OsuTools.Models.Scores {
    public class Replay : IScore {

        #region Constructor

        public Replay() { }

        public override string ToString() {
            return $"{Player} - {Ruleset} - {ModsUsed} - {Accuracy}";
        }
        #endregion

        #region IScore Interface

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

        private string modsUsed;
        #endregion

        #region Public Properties

        public string ScoreBarGraph { get; set; }

        public ReplayFrame[] Frames { get; set; }
        #endregion

        public static Replay Read(string path) {

            // If the .osr doesn't have replay data ( size < 2KB )
            // ScoreBarGraph & Frames will be null ( uninitialized )
            return (Replay)new BinaryReader(File.OpenRead(path)).ReadScore(true);
        }
    }
}
