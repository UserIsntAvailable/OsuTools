using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OsuTools.Models.Database {
    public class CollectionDatabase : IEnumerable<KeyValuePair<string, string[]>> {

        // This is good code and practices.... ( At least that's what I think )

        #region Private Fields

        private Dictionary<string, string[]> _collectionEntries;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a <see cref="CollectionDatabase"/> instance
        /// </summary>
        /// <param name="version">Database version</param>
        internal CollectionDatabase(int version) {

            Version = version;

            _collectionEntries = new();
        }

        /// <summary>
        /// Initialize a <see cref="CollectionDatabase"/> instance
        /// </summary>
        /// <param name="version">Database version</param>
        /// <param name="collection">The collection whose elements are copied to the <see cref="CollectionDatabase"/></param>
        internal CollectionDatabase(int version, IEnumerable<KeyValuePair<string, string[]>> collection) {

            Version = version;

            _collectionEntries = new(collection);
        }
        #endregion

        #region IEnumerable Implementation

        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator() {

            foreach (var collectionEntry in _collectionEntries) {

                yield return collectionEntry;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Public Properties

        public int Version { get; private set; }
        #endregion

        #region Indexers

        /// <summary>
        /// Gets the beatmaps hashes of the indexed collection
        /// </summary>
        /// <param name="key">The collection entry name</param>
        /// <returns>The beatmaps hashes of that collection</returns>
        public string[] this[string key] => _collectionEntries[key];

        /// <summary>
        /// Gets the collection entry with his index
        /// </summary>
        /// <param name="key">The collection index</param>
        /// <returns>The collection entry</returns>
        public KeyValuePair<string, string[]> this[int key] => _collectionEntries.ElementAt(key);
        #endregion

        #region Public Methods

        /// <summary>
        /// Add a collection entry to this <see cref="CollectionDatabase"/> instance
        /// If the collection already exists the <paramref name="beatmapHashes"/> would be added to it
        /// </summary>
        /// <param name="name">The name of the collection</param>
        /// <param name="beatmapHashes">The beatmaps that are inside the collection</param>
        public void AddEntry(string name, params string[] beatmapHashes)
            => _collectionEntries[name] = _collectionEntries[name].Concat(beatmapHashes).ToArray();

        /// <summary>
        /// Delete a collection entry to this <see cref="CollectionDatabase"/> instance
        /// </summary>
        /// <param name="name">The name of the collection</param>
        public void DeleteEntry(string name)
            => _collectionEntries.Remove(name);
        #endregion
    }
}
