using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OsuTools.Models.Beatmaps {
    public class Beatmap : IBeatmapInfo, IEquatable<Beatmap> {

        #region Constructor

        internal Beatmap() { }

        /// <summary>
        /// Change Beatmap string format
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            return $"{Artist} - {Title}[{Version}]({Creator}) {Ruleset}\n" +
                   $"CS: {CircleSize:F1}        AR: {ApproachRate:F1} " +
                   $"OD: {OverallDifficulty:F1} HP: {HPDrainRate:F1}";
        }

        public override bool Equals(object other) {

            if (!(other is Beatmap)) return false;

            return Equals((Beatmap)other);
        }

        public bool Equals([AllowNull] Beatmap other) {

            /* The osu!.db doesn't have all his data updated all the time because if the .osu file
             * of a beatmap is changed, the osu!.db don't will be updated until you rebuilt it.
              
             * This is why Equals need to verified differents things. */

            if (Scores == other.Scores && Hash == other.Hash) return true;

            // IDK how a beatmap can have differents BeatmapID's...
            else if (BeatmapID == other.BeatmapID) return true;

            else if (ToString() == other.ToString()) return true;

            return false;
        }

        public override int GetHashCode() {

            int hash = 19;

            hash = hash * 47 + Scores.GetHashCode();
            hash = hash * 47 + Ruleset.GetHashCode();
            hash = hash * 47 + Hash.GetHashCode();
            hash = hash * 47 + RankedStatus.GetHashCode();
            hash = hash * 47 + FilePath.GetHashCode();
            hash = hash * 47 + PreviewTime.GetHashCode();
            hash = hash * 47 + MaxCombo.GetHashCode();

            return hash;
        }
        #endregion

        #region Public Properties

        #region ScoreDatabaseParser Class

        internal IEnumerable<Score> Scores { get; set; }
        #endregion

        #region OsuFileParser/OsuDatabaseParser Class

        #region Properties that can be obtained in (.osu \ osu!.db \ OsuAPI)

        public Ruleset Ruleset { get; set; }

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
        public int BeatmapID { get; set; }    // default 0 (if the map is not ranked)
        public int BeatmapSetID { get; set; } // default -1 (if the map is not ranked)
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