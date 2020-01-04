using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using OsuTools.Models;
using OsuTools.Exceptions;
using OsuTools.Models.Beatmaps;
using static OsuTools.Utils.EnumHelper;

namespace OsuTools.Tools.Readers {
    public static class OsuFileReader {

        #region Private Properties

        /// <summary>
        /// Returned from Parse()
        /// </summary>
        private static readonly Beatmap Beatmap = new Beatmap();

        /// <summary>
        /// Sections that I will not use on this parser ( Simply because I don't need them )
        /// </summary>
        private static readonly List<string> uselessSections = new List<string> { "Colours", "Events", "Editor" };

        /// <summary>
        /// Properties that need to add values to a List instead of assign
        /// </summary>
        private static readonly List<string> genericProperties = new List<string> { "TimingPoints", "HitObjects" };

        #region Private Regex Patterns

        /// <summary>
        /// Sections such as [General], [Metadata], etc...
        /// </summary>
        private static readonly Regex sectionPattern = new Regex(@"^\[(\w+)\]$");

        /// <summary>
        /// Key: (ar), Value: (9.3), etc...
        /// </summary>
        private static readonly Regex keyvaluePattern = new Regex(@"^(\w+)\s*:\s*(.*)$");

        /// <summary>
        /// osu file format v12, 13, 14, etc...
        /// </summary>
        private static readonly Regex osuversionPattern = new Regex(@"^[\s\ufeff\x7f]*osu file format v(\d+)\s*$");

        /// <summary>
        /// A string.Empty
        /// </summary>
        private static readonly Regex blanklinePattern = new Regex(@"^[\s\ufeff\x7f]*$");
        #endregion
        #endregion

        /// <summary>
        /// Parse an .osu file
        /// </summary>
        /// <param name="path">The path of the .osu</param>
        /// <returns><see cref="Beatmap"> Object</returns>
        public static Beatmap Read(string path) {

            Match keyvalue;
            bool validFile = false;
            string currentSection = string.Empty;

            Beatmap.Hash = File.ReadAllBytes(path).ConvertToMD5();

            foreach (var line in File.ReadAllLines(path)) {

                if (line == "\n") continue;

                if (blanklinePattern.IsMatch(line)) continue;

                if (!validFile) {

                    Match versionMatch = osuversionPattern.Match(line);

                    if (versionMatch.Success) {

                        validFile = true;

                        int.TryParse(versionMatch.Groups[1].Value, out int version);

                        if (version < 4) {
                            throw new OsuFileVersionTooOldException($"The osu version of the file is too old. Version: {version}");
                        }
                        continue;
                    }

                    else {
                        throw new OsuFileFormatException($"The line: {line} isn't well formated");
                    }
                }

                Match section = sectionPattern.Match(line);

                if (section.Success) {
                    currentSection = section.Groups[1].Value;
                    continue;
                }

                if (uselessSections.Contains(currentSection)) continue;

                if (genericProperties.Contains(currentSection)) {
                    SetValueToBeatmapProperty(currentSection, line);
                }

                else {

                    keyvalue = keyvaluePattern.Match(line);
                    string key = keyvalue.Groups[1].Value;
                    string value = keyvalue.Groups[2].Value;

                    if (key == "Mode") {

                        Ruleset mode = (Ruleset)long.Parse(value);
                        SetValueToBeatmapProperty("Mode", mode);
                    }

                    else {
                        SetValueToBeatmapProperty(key, value);
                    }
                }
            }

            return Beatmap;
        }

        #region Private Methods

        /// <summary>
        /// Get the MD5 hash representation from this string
        /// </summary>
        /// <param name="text">Input that will be converted to MD5</param>
        /// <returns>Hash MD5 of the input</returns>
        private static string ConvertToMD5(this byte[] textBytes) {

            StringBuilder sb = new StringBuilder();

            MD5 md5 = MD5.Create();

            byte[] hash = md5.ComputeHash(textBytes);

            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Set a value to <see cref="Beatmap"> with the property names required.
        /// </summary>
        /// <param name="propertyName">The property name that have <see cref="Beatmap"></param>
        /// <param name="value">The value that you want set to the property.</param>
        private static void SetValueToBeatmapProperty(string propertyName, object value) {

            // Get the PropertyInfo of the Beatmap
            PropertyInfo property = typeof(Beatmap).GetProperty(propertyName);

            // If this property doesn't exits on the Beatmap Type, return
            if (property == null) return;

            // If is Generic ( List<string> ) => ( TimingPoints or HitObjects )
            if (property.PropertyType.IsGenericType) {

                // Get the current value of the List<string>
                var propertyList = property.GetValue(Beatmap);

                // Add value to the current List<string>
                propertyList.GetType()
                            .GetMethod("Add")
                            .Invoke(propertyList, new[] { (string)value });
            }

            else {

                // Convert value to the true Type of the property
                value = Convert.ChangeType(value, property.PropertyType);

                // Set value to the property 
                property.SetValue(Beatmap, value);
            }
        }
        #endregion
    }
}