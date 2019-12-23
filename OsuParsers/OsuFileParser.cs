using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OsuTools.Models;
using OsuTools.Exceptions;
using static OsuTools.Utils.EnumHelper;

namespace OsuParsers {
    public static class OsuFileParser {

        #region Private Properties

        private static Beatmap Beatmap { get; set; } = new Beatmap();

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
        /// <returns>Beatmap Object</returns>
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

                if (new List<string> { "Colours", "Events", "Editor" }.Contains(currentSection)) continue;

                if (new List<string> { "TimingPoints", "HitObjects" }.Contains(currentSection)) {
                    SetValueToBeatmapProperty(currentSection, line);
                }

                else {

                    keyvalue = keyvaluePattern.Match(line);
                    string key = keyvalue.Groups[1].Value;
                    string value = keyvalue.Groups[2].Value;

                    if (new List<string> { "AudioFilename", "AudioLeadIn", "PreviewTime", "Countdown", "SampleSet",
                        "StackLeniency", "LetterboxInBreaks", "WidescreenStoryboard", "TitleUnicode", "ArtistUnicode",
                        "EpilepsyWarning"}.Contains(key)) continue;

                    if (key == "Mode") {

                        GameMode mode = GetEnumField<GameMode>(long.Parse(value));
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
        /// Set a value to the beatmap object[line 13] with the property names required.
        /// </summary>
        /// <param name="propertyName">The property name that have the beatmap object [Beatmap Type]</param>
        /// <param name="value">The value that you want set to the property.
        ///                     WARNING: If you will use this make attention that the value set 
        ///                              is the actual value of the property.   </param>
        private static void SetValueToBeatmapProperty(string propertyName, object value) {

            PropertyInfo property = typeof(Beatmap).GetProperty(propertyName);

            if (property.PropertyType.IsGenericType) {

                var propertyList = property.GetValue(Beatmap);

                propertyList.GetType()
                            .GetMethod("Add")
                            ?.Invoke(propertyList, new[] { (string)value });
            }

            else {

                value = Convert.ChangeType(value, property.PropertyType);

                property.SetValue(Beatmap, value);
            }
        }
    }
}