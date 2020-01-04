namespace OsuTools.Models.Scores {
    public struct Hits {

        #region Constructor

        public Hits(short hit300, short hit100, short hit50, short gekis, short katus, short misses) {

            Hit300 = hit300;
            Hit100 = hit100;
            Hit50 = hit50;
            Gekis = gekis;
            Katus = katus;
            Misses = misses;
        }

        public override string ToString() {
            return $"300:{Hit300} | _300:{Gekis} | 100:{Hit100} | _100:{Katus} | 50:{Hit50} | Miss:{Misses}";
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