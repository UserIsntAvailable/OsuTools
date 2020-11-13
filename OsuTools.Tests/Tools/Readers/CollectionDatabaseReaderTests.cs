using Xunit;
using OsuTools.Tools.Readers;
using static OsuTools.Tests.Resources.ResourcesHelper;

namespace OsuTools.Tests.Tools.Readers {
    public class CollectionDatabaseReaderTests {

        [Fact]
        public void ReadingASingleFileTest() {

            var filePath = GetResourcePathFile("collection.db");

            var collDb = CollectionDatabaseReader.Read(filePath);

            Assert.Equal(2, collDb["Enumerable.Range"].Length);
            Assert.Equal(4, collDb["Such a good song"].Length);
        }
    }
}
