using System.Globalization;

namespace OsuTools.Models.Scores {
    public struct ReplayFrame {

        #region Constructor

        public ReplayFrame(long timeDiff, long currentTime, float x, float y, ReplayKeyStatus key) {

            TimeDiff = timeDiff;
            CurrentTime = currentTime;
            X = x;
            Y = y;
            Key = key;
        }

        public override string ToString() {
            return $"{CurrentTime}ms: {TimeDiff}|{X.ToString(CultureInfo.InvariantCulture)}|{Y.ToString(CultureInfo.InvariantCulture)}|{(int)Key}";
        }
        #endregion

        #region Properties

        /// <summary>
        /// Time in milliseconds since the previous action
        /// </summary>
        public long TimeDiff { get; internal set; }

        /// <summary>
        /// Time in milliseconds with the offset applied
        /// </summary>
        public long CurrentTime { get; internal set; }

        /// <summary>
        /// Horizontal position of the mouse between ( 0 and 512 )
        /// </summary>
        public float X { get; internal set; }

        /// <summary>
        /// Vertical position of the mouse bewteen ( 0 and 384 )
        /// </summary>
        public float Y { get; internal set; }

        /// <summary>
        /// Key pressed <see cref="ReplayKeyStatus"/> on the current frame
        /// </summary>
        public ReplayKeyStatus Key { get; internal set; }
        #endregion
    }
}
