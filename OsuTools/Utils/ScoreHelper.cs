using System;
using System.Text;
using OsuTools.Models;

#nullable enable
namespace OsuTools.Utils {
    internal static class ScoreHelper {

        /// <summary>
        /// Get the Accuracy in %
        /// </summary>
        /// <param name="c300">Number of 300</param>
        /// <param name="c100">Number of 100</param>
        /// <param name="c50">Number of 50</param>
        /// <param name="misses">Number of misses</param>
        /// <returns>Acc in %</returns>
        public static double GetAcc(uint c300, uint c100, uint c50, uint misses) {

            // Get all the TotalHits without Gekis and Katus.
            uint TotalHits = c300 + c100 + c50 + misses;

            return Math.Round(((double)(c50 + c100 * 2 + c300 * 6) / (TotalHits * 6)) * 100, 2);
        }

        /// <summary>
        /// Get the Accuracy in %
        /// </summary>
        /// <param name="hits">Hits instance</param>
        /// <returns>Acc in %</returns>
        internal static double GetAcc(Hits hits) {

            // Get all the TotalHits without Gekis and Katus.
            int TotalHits = hits.Hit300 + hits.Hit100 + hits.Hit50 + hits.Misses;

            if (TotalHits <= 0) return 0.0;

            return Math.Round(((double)(hits.Hit50 + hits.Hit100 * 2 + hits.Hit300 * 6) / (TotalHits * 6)) * 100, 2);
        }

        /// <summary>
        /// Transform Timestamp to Datetime date
        /// </summary>
        /// <param name="timestand"></param>
        /// <returns>Date format (Month, Day, Year, Hours, Minutes, Seconds)</returns>
        public static DateTime GetDate(long timestand)
            => new DateTime(timestand - 144000000000, DateTimeKind.Unspecified);

        /// <summary>
        /// Convert a mod bitwise into his string representation abbreviated 
        /// </summary>
        /// <param name="bitwise">Mod bitwise</param>
        /// <returns>String representation abbreviated</returns>
        public static string BitwiseToString(long bitwise) {

            StringBuilder modsSB = new StringBuilder();

            string? mods = Enum.Parse(typeof(Mods), bitwise.ToString()).ToString();

            // Return NM if somehow mods is null
            if (mods == null || bitwise == 0) {
                return modsSB.Append("NM").ToString();
            }

            // This is the good order of the mods : ) (ex HDDT not DTHD)
            else if (!int.TryParse(mods, out _)) {
                if (mods.Contains("ScoreV2")) {
                    modsSB.Append("V2");
                }
                if (mods.Contains("TouchDevice")) {
                    modsSB.Append("TD");
                }
                if (mods.Contains("Cinema")) {
                    modsSB.Append("CN");
                }
                if (mods.Contains("Auto")) {
                    modsSB.Append("AU");
                }
                if (mods.Contains("Relax")) {
                    modsSB.Append("RX");
                }
                if (mods.Contains("AutoPilot")) {
                    modsSB.Append("AP");
                }
                if (mods.Contains("TargetPractice")) {
                    modsSB.Append("TP");
                }
                if (mods.Contains("SpunOut")) {
                    modsSB.Append("SO");
                }
                if (mods.Contains("Easy")) {
                    modsSB.Append("EZ");
                }
                if (mods.Contains("NoFail")) {
                    modsSB.Append("NF");
                }
                if (mods.Contains("Hidden")) {
                    modsSB.Append("HD");
                }
                if (mods.Contains("HalfTime")) {
                    modsSB.Append("HT");
                }
                if (mods.Contains("DoubleTime")) {
                    modsSB.Append("DT");
                }
                if (mods.Contains("Nightcore")) {
                    modsSB.Append("NC");
                }
                if (mods.Contains("HardRock")) {
                    modsSB.Append("HR");
                }
                if (mods.Contains("SuddenDeath")) {
                    modsSB.Append("SD");
                }
                if (mods.Contains("Perfect")) {
                    modsSB.Append("PF");
                }
                if (mods.Contains("FlashLight")) {
                    modsSB.Append("FL");
                }
            }

            else {
                throw new Exception("This bitwise doesn't exit");
            }

            return modsSB.ToString();
        }
    }
}
