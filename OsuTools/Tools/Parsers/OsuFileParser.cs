using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OsuTools.Models;
using OsuTools.Exceptions;
using OsuTools.Models.Beatmaps;
using static OsuTools.Utils.EnumHelper;

namespace OsuTools.Tools.Parsers {
    public static class OsuFileParser {

        #region Private Properties

        // Returned from Parse()
        private static Beatmap Beatmap { get; set; } = new Beatmap();

        // Sections that I will not use on this parser ( Simply because I don't need them )
        private static List<string> UselessSections { get; set; } = new List<string> { "Colours", "Events", "Editor" };

        // Properties that need to add values to a List instead of assign
        private static List<string> GenericProperties { get; set; } = new List<string> { "TimingPoints", "HitObjects" };

        #region Private Regex Patterns

        // Sections such as [General], [Metadata], etc...
        private static readonly Regex sectionPattern = new Regex(@"^\[(\w+)\]$");

        // Key: (ar), Value: (9.3), etc...
        private static readonly Regex keyvaluePattern = new Regex(@"^(\w+)\s*:\s*(.*)$");

        // osu file format v12, 13, 14, etc...
        private static readonly Regex osuversionPattern = new Regex(@"^[\s\ufeff\x7f]*osu file format v(\d+)\s*$");

        // A string.Empty
        private static readonly Regex blanklinePattern = new Regex(@"^[\s\ufeff\x7f]*$");
        #endregion
        #endregion

        /// <summary>
        /// Parse an .osu file
        /// </summary>
        /// <param name="path">The path of the .osu</param>
        /// <returns><see cref="Beatmap"> Object</returns>
        public static Beatmap Parse(string path) {

            Match keyvalue;
            bool validFile = false;
            string currentSection = string.Empty;

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

                if (UselessSections.Contains(currentSection)) continue;

                if (GenericProperties.Contains(currentSection)) {
                    SetValueToBeatmapProperty(currentSection, line);
                }

                else {

                    keyvalue = keyvaluePattern.Match(line);
                    string key = keyvalue.Groups[1].Value;
                    string value = keyvalue.Groups[2].Value;

                    if (key == "Mode") {

                        Ruleset mode = GetEnumField<Ruleset>(long.Parse(value));
                        SetValueToBeatmapProperty("Mode", mode);
                    }

                    else {
                        SetValueToBeatmapProperty(key, value);
                    }
                }
            }

            return Beatmap;
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
    }
}