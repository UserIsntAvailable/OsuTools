using OsuTools.Models.Enums;

namespace OsuTools.Models.Scores {
    public class ReplayFrame {

        public override string ToString() 
            => $"{CurrentTime}ms: {TimeDiff}|{MousePosition.X}|{MousePosition.Y}|{Key.ToString("D")}";

        #region Public Properties

        /// <summary>
        /// Time in milliseconds since the previous action
        /// </summary>
        public long TimeDiff { get; internal set; }

        /// <summary>
        /// Time in milliseconds with the offset applied
        /// </summary>
        public long CurrentTime { get; internal set; }

        /// <summary>
        /// X: Horizontal position of the mouse between ( 0 and 512 )
        /// Y: Vertical position of the mouse bewteen ( 0 and 384 )
        /// </summary>
        public (float X, float Y) MousePosition { get; internal set; }

        /// <summary>
        /// Key pressed <see cref="ReplayKeyStatus"/> on the current frame
        /// </summary>
        public ReplayKeyStatus Key { get; internal set; }
        #endregion
    }
}
