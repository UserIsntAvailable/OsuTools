﻿using System.Collections;
using System.Collections.Generic;

#if TESTMODE
namespace OsuTools.Models.Database {

    public class OsuDatabase : IEnumerable<Beatmap> {
        
        #region IEnumerable Interface

        public IEnumerator<Beatmap> GetEnumerator() {
            
            foreach (var beatmap in Beatmaps) {
                yield return beatmap;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion

        public int Version { get; set; }

        public Beatmap[] Beatmaps { get; set; }
    }

}
#endif
