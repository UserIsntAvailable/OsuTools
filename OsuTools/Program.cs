using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using OsuTools.Models.Beatmaps.Sections;
using OsuTools.Models.Database;
using OsuTools.Models.Enums;
using OsuTools.Models.Scores;
using OsuTools.Tools;
using OsuTools.Tools.Readers;
using OsuTools.Utils;

namespace OsuTools {

    struct Program {

        static void Main(string[] args) {

            string path = @"C:\Users\Angel Pineda\AppData\Local\osu!\Songs\749304 USAO - FREEDOM (Extended Mix)\USAO - FREEDOM (Extended Mix) (C00L) [Insane Collab].osu";

            //Replay replay = Replay.Read(@"C:\Users\Angel Pineda\AppData\Local\osu!\Replays\ANDavid1611 - otetsu - Minamo no Sakura, Yume wa Sakayume [Reversed Dream] (2019-08-08) Osu.osr");

            //double maplength = Math.Round(2382.2, 2);
            //double replaylength = Math.Round((double)replay.Frames[^1].CurrentTime / 60, 2);

            //Console.WriteLine(maplength);
            //Console.WriteLine(replaylength);

            //foreach (var item in replay.Frames) {

            //    Console.WriteLine(item);

            //    if (item.CurrentTime == 132666) {
            //        Console.WriteLine(item + " " + "hey");
            //    }
            //}

            var test = File.ReadAllLines(path);

            Stopwatch stpw = new Stopwatch();
            stpw.Start();

            var gS = test.GeneralSectionParser();
            var eS = test.EditorSectionParser();
            var mS = test.MetadataSectionParser();
            var dS = test.DifficultySectionParser();
            var tpS = test.TimingPointsSectionParser();

            stpw.Stop();
            Console.WriteLine(stpw.ElapsedMilliseconds);

            Console.WriteLine($"{gS}\n");
            Console.WriteLine($"{eS}\n");
            Console.WriteLine($"{mS}\n");
            Console.WriteLine($"{dS}\n");
            foreach (var item in tpS) {
                Console.WriteLine(item);
            }
        }
    }
}