using System;
using OsuTools.Models.Enums;

#nullable enable
namespace OsuTools.Utils {
    internal static class EnumHelper {

        /// <summary>
        /// Transform a string to Mods type ( that can be also transform to bitwise )
        /// </summary>
        /// <param name="mods">The string abbreviated of the mod ( ex. HDDT )</param>
        /// <returns>Mod type representation of the string</returns>
        public static Mods StringToMods(string? mods) {

            Mods mod = Mods.NoMod;

            for (int i = 0; i < mods?.Length; i += 2) {
                mod += (mods[i..(i + 2)]) switch
                {
                    "NF" => (int)Mods.NoFail,
                    "EZ" => (int)Mods.Easy,
                    "TD" => (int)Mods.TouchDevice,
                    "HD" => (int)Mods.Hidden,
                    "HR" => (int)Mods.HardRock,
                    "SD" => (int)Mods.SuddenDeath,
                    "DT" => (int)Mods.DoubleTime,
                    "RX" => (int)Mods.Relax,
                    "HT" => (int)Mods.HalfTime,
                    "NC" => (int)Mods.NightCore,
                    "FL" => (int)Mods.FlashLight,
                    "AU" => (int)Mods.Auto,
                    "SO" => (int)Mods.SpunOut,
                    "AP" => (int)Mods.AutoPilot,
                    "PF" => (int)Mods.Perfect,
                    "CN" => (int)Mods.Cinema,
                    "TP" => (int)Mods.TargetPractice,
                    "V2" => (int)Mods.ScoreV2,
                    _ => throw new ArgumentException($"This mod doesn't exit {mods[i..(i + 2)]}", mods),
                };
            }

            return mod;
        }
    }
}
