using System.IO;
using System.Text;
using OsuTools.Exceptions;

namespace OsuTools.Utils {
    internal static class BinaryReaderHelper {

        /// <summary>
        /// Get an Osu! string from this binary reader
        /// </summary>
        /// <param name="reader">The binary reader</param>
        /// <returns>The osu string</returns>
        internal static string ReadOsuString(this BinaryReader reader) {

            /// <summary>
            /// Get un Unsigned Little Endian base 128 interger from the file
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

            throw new StringNotValidException("Couldn't not read valid string from .db file");
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

