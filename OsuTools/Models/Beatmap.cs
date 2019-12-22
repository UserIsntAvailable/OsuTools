using System;
using System.Collections.Generic;
using System.Linq;

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

        #region Score Class

        public int NumScores { get { return Scores.Count(); } }
        public IEnumerable<Score> Scores { get; internal set; }

        #endregion

        #region OsuFileParser/OsuDatabase Class

        #region Properties that can be obtained in (.osu \ osu!.db \ OsuAPI)

        public GameMode Mode { get; set; }       // 0 for Standard, 1 for Taiko, 2 for Catch The Beat, 3 for Mania

        #region Difficulty Vars (These are the default values)
        public double ApproachRate { get; set; } = 5.0;
        public double CircleSize { get; set; } = 5.0;
        public double HPDrainRate { get; set; } = 5.0;
        public double OverallDifficulty { get; set; } = 5.0;
        public double SliderMultiplier { get; set; } = 1.0;  // Slider Velocity and Slider Multiplier are the same
        public double SliderTickRate { get; set; } = 1;
        #endregion

        #region Metadata Properties
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Creator { get; set; }
        public string Version { get; set; }
        public string Source { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public int BeatmapID { get; set; }    // default 0 (if the map is not ranked)
        public int BeatmapSetID { get; set; } // default -1 (if the map is not ranked)
        #endregion

        public List<string> TimingPoints { get; set; } = new List<string>();

        public List<string> HitObjects { get; set; } = new List<string>();       // Only in .osu
        #endregion

        #region Properties that can be obtained only in the (osu!.db \ OsuAPI)


        public double StarRating {

            get { return StarRatingR; }

            set { StarRatingR = Math.Round(value, 1); }
        }                    // SR of the gamemode map ( Also obtained in (field) Stars )
        public string Hash { get; set; }                // Beatmap hash
        public RankedStatus RankedStatus { get; set; }  // (4=ranked, 5=approved, 2=pending/graveyard) and loved?

        public int NbObjects {
            get {
                return NbCircles + NbSliders + NbSpinners;
            }
        }                        // Total of hitcircles + sliders + spinners
        public int Length { get; set; }                 // Length of the map in milliseconds (seconds if OsuAPI Object)
        public int NbCircles { get; set; }              // Maximum number of circles
        public int NbSliders { get; set; }              // Maximum number of sliders
        public int NbSpinners { get; set; }             // Maximum number of spinners
        #endregion

        #region Properties that can be obtained only in the osu!.db file

        public string FilePath { get; set; }            // .osu file path
        public int PreviewTime { get; set; }            // Time when the audio preview when hovering over a beatmap in beatmap select starts, in milliseconds.       
        #endregion

        #region Properties that can be obtained only in osuAPI

        public int MaxCombo { get; set; }                             // Max Combo possible obtained in the map
        public double PeformancePoints { get; set; }                  // Total PP of the map
        public List<double> Stars { get; set; } = new List<double>(); // StarRating, Aim and Speed Stars
        #endregion

        #region Property that need be calculated

        public int MaxScore { get; set; }               // Max Score possible obtained in the map
        #endregion
        #endregion
        #endregion

        #region Private Property

        private double StarRatingR { get; set; }
        #endregion
    }
}