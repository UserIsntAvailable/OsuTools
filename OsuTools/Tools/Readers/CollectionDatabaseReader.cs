using System.IO;
using System.Linq;
using System.Collections.Generic;
using OsuTools.Utils;
using OsuTools.Models.Database;

namespace OsuTools.Tools.Readers {
    public static class CollectionDatabaseReader {

        // This is good code and practices.... ( At least that's what I think )

        #region Public Methods

        public static CollectionDatabase Read(string path) {

            var reader = new BinaryReader(File.Open(path, FileMode.Open));

            var dbVersion = reader.ReadInt32();
            var collectionEntries =
            // _reader.ReadInt32() = # of collections
            Enumerable.Range(0, reader.ReadInt32())
                .Select(_ => KeyValuePair.Create(
                    // _reader.ReadOsuString() = Collection name
                    reader.ReadOsuString(),
                    // _reader.ReadInt32() = # of beatmaps on that collection
                    Enumerable.Range(0, reader.ReadInt32())
                        // _reader.ReadOsuString() = Beatmap Hash
                        .Select(_ => reader.ReadOsuString())
                        .ToArray())
                );

            return new(dbVersion, collectionEntries);
        }
        #endregion
    }
}
