namespace OsuTools.Models {
    public class Hits {

        #region Constructor

        internal Hits() { }

        public override string ToString() {
            return $"300:{Hit300} | _300:{Gekis} | 100:{Hit100} | _100:{Katus} | 50:{Hit50} | Miss:{Misses}\n";
        }
        #endregion

        #region Properties

        public short Hit300 { get; internal set; }

        public short Hit100 { get; internal set; }

        public short Hit50 { get; internal set; }

        public short Gekis { get; internal set; }

        public short Katus { get; internal set; }

        public short Misses { get; internal set; }
        #endregion
    }
}