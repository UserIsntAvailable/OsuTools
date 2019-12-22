using System.Collections.Generic;
using System.Linq;

namespace OsuTools.Models {
    public class ScoreDatabase {

        /// <summary>
        /// Constructor
        /// </summary>
        internal ScoreDatabase() { }

        #region Properties

        public int NumBeatmaps { get { return Beatmaps.Count(); } }
        public IEnumerable<Beatmap> Beatmaps { get; internal set; }
        #endregion
    }
}