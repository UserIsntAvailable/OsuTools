using SevenZip;
using System.IO;
using System.Text;
using OsuTools.Exceptions;
using OsuTools.Models.Enums;
using OsuTools.Models.Scores;

namespace OsuTools.Utils {
    internal static class BinaryReaderHelper {

        /// <summary>
        /// Read a <see cref="Replay"> or <see cref="Score"> from this reader
        /// </summary>
        /// <param name="reader">The binary reader</param>
        /// <param name="isReplay">Specified is this will read a Replay</param>
        /// <returns>IScore intefarce that will be casted later</returns>
        internal static IScore ReadScore(this BinaryReader reader, bool isReplay) {

            // Using the dinamic object slow down the fuction
            // IDK if I will change this on the future from 
            // 165ms this is 50ms with no dynamic object
            dynamic score;

            if (isReplay) {
                score = new Replay();
            }
            else {
                score = new Score();
            }

            byte[] compressedData = new byte[0];


            score.Ruleset = (Ruleset)reader.ReadByte();

            // Version of this score (e.g. 20150203)
            int version = reader.ReadInt32();

            score.BeatmapHash = reader.ReadOsuString();
            score.Player = reader.ReadOsuString();
            score.Hash = reader.ReadOsuString();

            #region Hits Reader

            var hit300 = reader.ReadInt16();
            var hit100 = reader.ReadInt16();
            var hit50 = reader.ReadInt16();
            var gekis = reader.ReadInt16();
            var katus = reader.ReadInt16();
            var misses = reader.ReadInt16();

            score.Hits = new Hits(hit300, hit100, hit50, gekis, katus, misses);
            #endregion

            score.ScoreObtained = reader.ReadInt32();
            score.MaxCombo = reader.ReadInt16();
            score.PerfectCombo = reader.ReadBoolean();
            score.ModsUsed = reader.ReadInt32().ToString();

            if (isReplay) {
                score.ScoreBarGraph = reader.ReadOsuString();
            }
            else {

                // Should always be empty
                reader.ReadOsuString();
            }

            score.Timestand = reader.ReadInt64();

            if (isReplay) {

                // Length in bytes of compressed replay data
                int length = reader.ReadInt32();

                // Compressed replay data
                compressedData = reader.ReadBytes(length);
            }
            else {

                // Int (Should always be 0xffffffff [-1])
                reader.ReadBytes(4);
            }

            if (version >= 20140721) {
                score.OnlineScoreID = reader.ReadInt64();
            }
            else if (version >= 20121008) {
                score.OnlineScoreID = reader.ReadInt32();
            }

            if (score.ModsUsed.Contains("TP")) {
                // Total accuracy of all hits
                reader.ReadDouble();
            }

            // The file can be .osr but it may not have the replay data
            if (isReplay && compressedData.Length != 0) {

                using var ms = new MemoryStream(compressedData);
                MemoryStream os = LZMACoder.Decompress(ms);

                long lastTime = 0;

                string[] actions = new StreamReader(os).ReadToEnd().Split(",");

                // actions.Length - 2 because I don't need the RNGSeed of the end 
                score.Frames = new ReplayFrame[actions.Length - 2];

                for (long i = 0; i < actions.Length - 2; i++) {

                    var split = actions[i].Split("|");

                    lastTime += long.Parse(split[0]);

                    score.Frames[i] = new ReplayFrame() {
                        Key = (ReplayKeyStatus)int.Parse(split[3]),
                        TimeDiff = long.Parse(split[0]),
                        CurrentTime = lastTime,
                        MousePosition = (float.Parse(split[1]), float.Parse(split[2])),
                    };
                }
            }

            return score;
        }

        /// <summary>
        /// Read an Osu! string from this binary reader
        /// </summary>
        /// <param name="reader">The binary reader</param>
        /// <returns>The osu string</returns>
        internal static string ReadOsuString(this BinaryReader reader) {

            /// <summary>
            /// Get an Unsigned Little Endian base 128 interger from the file
            /// </summary>
            /// <param name="reader">The binary reader</param>
            /// <returns>Int buffer</returns>
            static int ParseUleb128(BinaryReader reader) {
                int result = 0;
                int shift = 0;

                while (true) {
                    byte b = reader.ReadByte();
                    result |= (b & 0x7f) << shift;

                    if (((b & 0x80) >> 7) == 0)
                        break;

                    shift += 7;
                }
                return result;
            }

            // Get next byte, this one indicates what the rest of the string is
            byte indicator = reader.ReadByte();

            switch (indicator) {
                case 0:
                    // The next two parts are not present
                    return "";
                case 11:
                    // The next two parts are present
                    int uleb = ParseUleb128(reader);
                    byte[] buffer = reader.ReadBytes(uleb);
                    string a = Encoding.UTF8.GetString(buffer);
                    return a;
            }

            throw new StringNotValidException("Couldn't not read valid string from osu file");
        }

        /// <summary>
        /// Read an Int-DoublePair from this binary reader <see cref="https://osu.ppy.sh/help/wiki/osu%21_File_Formats/Osr_%28file_format%29">
        /// </summary>
        /// <param name="reader">The binary reader</param>
        /// <returns>Start Rating of the current bitwise combination</returns>
        internal static double ReadIntDoublePair(this BinaryReader reader) {

            // (Byte) Unknown
            // (Int)  Current bitwise combination. See: (https://osu.ppy.sh/help/wiki/osu%21_File_Formats/Osr_%28file_format%29)
            // (Byte) Unknown
            reader.ReadBytes(6);

            double starRating = reader.ReadDouble();

            return starRating;
        }
    }
}

