using System.Collections.Generic;

namespace OsuTools.Models {
    public class Beatmap {

        #region Constructor

        internal Beatmap() { }

        /// <summary>
        /// Change Beatmap string format
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            return $"{Artist} - {Title}[{Version}]({Creator}) {Mode}\n" +
                   $"CS: {CircleSize} AR: {ApproachRate} " +
                   $"OD: {OverallDifficulty} HP: {HPDrainRate}";
        }
        #endregion

        #region Public Properties

        #region ScoreDatabaseParser Class

        internal IEnumerable<Score> Scores { get; set; }
        #endregion

        #region OsuFileParser/OsuDatabaseParser Class

        #region Properties that can be obtained in (.osu \ osu!.db \ OsuAPI)

        public Ruleset Mode { get; set; }

        #region Difficulty Properties

        public double ApproachRate { get; set; } = 5.0;
        public double CircleSize { get; set; } = 5.0;
        public double HPDrainRate { get; set; } = 5.0;
        public double OverallDifficulty { get; set; } = 5.0;
        public double SliderMultiplier { get; set; } = 1.0;  // Slider Velocity and Slider Multiplier names are the same ( I prefer SliderMultiplier thought )
        public double SliderTickRate { get; set; } = 1;
        #endregion

        #region Metadata Properties

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Creator { get; set; }
        public string Version { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public int BeatmapId { get; set; }    // default 0 (if the map is not ranked)
        public int BeatmapSetId { get; set; } // default -1 (if the map is not ranked)
        #endregion

        public List<string> TimingPoints { get; set; } = new List<string>();
        public List<string> HitObjects { get; set; } = new List<string>();       // Only in .osu
        #endregion

        #region Properties that can be obtained only in the (osu!.db \ OsuAPI)

        public double StarRating { get; set; }          // SR of the gamemode map ( Also obtained in (field) Stars )
        public string Hash { get; set; }                // Beatmap hash
        public RankedStatus RankedStatus { get; set; }  
        public int NbObjects {
            get {
                return NbCircles + NbSliders + NbSpinners;
            }
        }                        // Total of hitcircles + sliders + spinners
        public int Length { get; set; }                 // Length of the map in milliseconds (seconds if returned from OsuAPI)
        public int NbCircles { get; set; }              // Maximum number of circles
        public int NbSliders { get; set; }              // Maximum number of sliders
        public int NbSpinners { get; set; }             // Maximum number of spinners
        #endregion

        #region Properties that can be obtained only in the osu!.db file

        public string FilePath { get; set; }            // .osu file path
        public int PreviewTime { get; set; }            // Time when hovering over a beatmap in beatmap select starts, in milliseconds.       
        #endregion

        #region Properties that can be obtained only in osuAPI

        public int? MaxCombo { get; set; }                             // Max Combo possible obtained in the map
        public double? PeformancePoints { get; set; }                  // Total PP of the map
        public List<double> Stars { get; set; } = new List<double>();  // StarRating, Aim and Speed Stars
        #endregion

        #region Property that need be calculated

        /* In reality, nearly all the properties can be obtained only with his
            .osu file ( I will make all that later [nearly all is calculated
             with the PeformancePointsCalculator]).*/

        public int MaxScore { get; set; }               // Max Score possible obtained in the map
        #endregion
        #endregion
        #endregion
    }
}