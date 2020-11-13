namespace OsuTools.Models.Scores {
    public class Hits {

        // Change constructor implementation

        public Hits(short hit300, short hit100, short hit50, short gekis, short katus, short misses)
            => (Hit300, Hit100, Hit50, Gekis, Katus, Misses) = (hit300, hit100, hit50, gekis, katus, misses);

        #region Public Properties

        public short Hit300 { get; set; }

        public short Hit100 { get; set; }

        public short Hit50 { get; set; }

        public short Gekis { get; set; }

        public short Katus { get; set; }

        public short Misses { get; set; }
        #endregion

        public override string ToString()
            => $"300:{Hit300} | _300:{Gekis} | 100:{Hit100} | _100:{Katus} | 50:{Hit50} | Miss:{Misses}";
    }
}