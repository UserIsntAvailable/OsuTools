using System;
using System.IO;
using System.Linq;
using OsuTools.Utils;
using OsuTools.Models;
using OsuTools.Models.Database;
using OsuTools.Models.Beatmaps;
using static OsuTools.Utils.EnumHelper;

namespace OsuTools.Tools.Readers {
    public static class OsuDatabaseReader {

        #region Private fields

        /// <summary>
        /// Reader used from Parse.
        /// </summary>
        private static BinaryReader Reader;

        /// <summary>
        /// The osu! version (e.g. 20150203)
        /// </summary>
        internal static int dbVersion;

        #region Stars Rating

        private static double starRatingStandard;

        private static double starRatingTaiko;

        private static double starRatingCTB;

        private static double starRatingMania;
        #endregion
        #endregion

        /// <summary>
        /// Parse an osu!.db
        /// </summary>
        /// <param name="path">The path of the osu.db</param>
        /// <returns><see cref="OsuDatabase"></returns>
        public static OsuDatabase Read(string path) {

            Reader = new BinaryReader(File.OpenRead(path));

            dbVersion = Reader.ReadInt32();

            /* (Int)    Number of Folders in Songs
             * (Bool)   AccountUnlocked ( false when account is locked or banned )
             * (Double) Date the accaunt will be unlocked */
            Reader.ReadBytes(13);

            // Player name
            Reader.ReadOsuString();

            OsuDatabase osuDatabase = new OsuDatabase() {

                Version = dbVersion,

                // Reader.ReadInt32() = The total number of Beatmaps in the ScoreDatabase
                Beatmaps = from _ in Enumerable.Range(0, Reader.ReadInt32()) select GetBeatmap()
            };

            return osuDatabase;
        }

        /// <summary>
        /// Get a Beatmap from the osu!.db
        /// </summary>
        /// <returns><see cref="Beatmap"></returns>
        private static Beatmap GetBeatmap() {

            var beatmap = new Beatmap();

            if (dbVersion < 20191106) {

                // Size in bytes of the beatmap entry
                Reader.ReadInt32();
            }

            beatmap.Artist = Reader.ReadOsuString();

            // Artist, in Unicode
            Reader.ReadOsuString();

            beatmap.Title = Reader.ReadOsuString();

            // Song Title, in Unicode
            Reader.ReadOsuString();

            beatmap.Creator = Reader.ReadOsuString();

            beatmap.Version = Reader.ReadOsuString();

            // Audio File
            Reader.ReadOsuString();

            beatmap.Hash = Reader.ReadOsuString();

            // The .osu name of the beatmap
            var osuFile = Reader.ReadOsuString();

            beatmap.RankedStatus = GetEnumField<RankedStatus>(Reader.ReadByte());
            beatmap.NbCircles = Reader.ReadInt16();
            beatmap.NbSliders = Reader.ReadInt16();
            beatmap.NbSpinners = Reader.ReadInt16();

            // (Long) Last modification time ( Windows Ticks )
            Reader.ReadBytes(8);

            // Difficulty, if dbVersion < 20140609 ReadByte otherwise ReadSingle
            if (dbVersion < 20140609) {

                beatmap.ApproachRate = Reader.ReadByte();
                beatmap.CircleSize = Reader.ReadByte();
                beatmap.HPDrainRate = Reader.ReadByte();
                beatmap.OverallDifficulty = Reader.ReadByte();
            }

            else {

                beatmap.ApproachRate = Reader.ReadSingle();
                beatmap.CircleSize = Reader.ReadSingle();
                beatmap.HPDrainRate = Reader.ReadSingle();
                beatmap.OverallDifficulty = Reader.ReadSingle();
            }

            beatmap.SliderMultiplier = Reader.ReadDouble();

            /* Int-Double Pair parser. See: (https://osu.ppy.sh/help/wiki/osu%21_File_Formats/Db_%28file_format%29)
             * If you want understand this, I will make a chart after
             * or you can debug this by yourself. */
            if (dbVersion >= 20140609) {

                #region Standard Star Rating

                // Number of Int-Double Pair of Standard to read
                int nbIDPStardard = Reader.ReadInt32();

                if (nbIDPStardard != 0) {

                    // NoMod start rating of standard mode
                    starRatingStandard = Reader.ReadIntDoublePair();

                    // Others star rating bitwise combination of standard mode
                    for (int i = 0; i < (nbIDPStardard - 1); i++) {
                        Reader.ReadIntDoublePair();
                    }
                }
                #endregion

                #region Taiko Star Rating

                // Number of Int-Double Pair of Taiko to read
                int nbIDPTaiko = Reader.ReadInt32();

                if (nbIDPTaiko != 0) {

                    // NoMod start rating of taiko mode
                    starRatingTaiko = Reader.ReadIntDoublePair();

                    // Others star rating bitwise combination of taiko mode
                    for (int i = 0; i < (nbIDPTaiko - 1); i++) {
                        Reader.ReadIntDoublePair();
                    }
                }
                #endregion

                #region CatchTheBeat Star Rating

                // Number of Int-Double Pair of CTB to read
                int nbIDPCTB = Reader.ReadInt32();

                if (nbIDPCTB != 0) {

                    // NoMod start rating of CTB mode
                    starRatingCTB = Reader.ReadIntDoublePair();

                    // Others star rating bitwise combination of CTB mode
                    for (int i = 0; i < (nbIDPCTB - 1); i++) {
                        Reader.ReadIntDoublePair();
                    }
                }
                #endregion

                #region Mania Star Rating

                // Number of Int-Double Pair of mania to read
                int nbIDPMania = Reader.ReadInt32();

                if (nbIDPMania != 0) {

                    // NoMod start rating of mania mode
                    starRatingMania = Reader.ReadIntDoublePair();

                    // Others star rating bitwise combination of mania mode
                    for (int i = 0; i < (nbIDPMania - 1); i++) {
                        Reader.ReadIntDoublePair();
                    }
                }
                #endregion
            }

            // (Int) Drain Time ( seconds )
            Reader.ReadBytes(4);

            beatmap.Length = Reader.ReadInt32();
            beatmap.PreviewTime = Reader.ReadInt32();

            // Number of TimePoints
            int nbTimingPoints = Reader.ReadInt32();

            /* (Double)  BPM
             * (Double)  Offset
             * (Boolean) TimingPoint is inherited or not? */
            for (int i = 0; i < nbTimingPoints; i++) {
                Reader.ReadBytes(17);
            }

            beatmap.BeatmapID = Reader.ReadInt32();
            beatmap.BeatmapSetID = Reader.ReadInt32();

            /* (Int)   Thread ID
             * (Byte)  Grade achieved in osu!Standard
             * (Byte)  Grade achieved in Taiko
             * (Byte)  Grade achieved in CTB
             * (Byte)  Grade achieved in osu!Mania
             * (Short) Local beatmap offset
             * (Float) Stack leniency */
            Reader.ReadBytes(14);

            beatmap.Ruleset = GetEnumField<Ruleset>(Reader.ReadByte());

            // Select the right starRating
            switch (beatmap.Ruleset) {

                case Ruleset.Standard:
                    beatmap.StarRating = starRatingStandard;
                    break;

                case Ruleset.Taiko:
                    beatmap.StarRating = starRatingTaiko;
                    break;

                case Ruleset.CatchTheBeat:
                    beatmap.StarRating = starRatingCTB;
                    break;

                case Ruleset.Mania:
                    beatmap.StarRating = starRatingMania;
                    break;
            }

            beatmap.Source = Reader.ReadOsuString();

            beatmap.Tags = Reader.ReadOsuString();

            // (Short) Online offset
            Reader.ReadBytes(2);

            // Font used for the title of the song
            Reader.ReadOsuString();

            /* (Boolean) Is the beatmap unplayed
             * (Long)    Last time played
             * (Boolean) Is beatmap osz2 */
            Reader.ReadBytes(10);

            // Reader.ReadOsuString() = Folder name of beatmap, relative to Songs folder
            beatmap.FilePath = Path.Join(Reader.ReadOsuString(), osuFile);

            /* (Long)    Last time when map was checked with osu! repository
             * (Boolean) Ignore beatmap sounds
             * (Boolean) Ignore beatmap skin
             * (Boolean) Disable storyboard
             * (Boolean) Disable video
             * (Boolean) Visual override */
            Reader.ReadBytes(13);

            // (Short) Unknown
            if (dbVersion < 20140609) {
                Reader.ReadBytes(2);
            }

            // (Int) Unknown
            Reader.ReadBytes(4);

            // (Byte) Mania Scroll Speed
            Reader.ReadByte();

            return beatmap;
        }
    }
}
