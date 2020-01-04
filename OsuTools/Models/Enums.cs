﻿using System;

namespace OsuTools.Models {

    [Flags]
    public enum Mods {

        NoMod          = 0,
        NoFail         = 1,
        Easy           = 2,
        TouchDevice    = 4,
        Hidden         = 8,
        HardRock       = 16,
        SuddenDeath    = 32,
        DoubleTime     = 64,
        Relax          = 128,
        HalfTime       = 256,
        NightCore      = 576,
        FlashLight     = 1024,
        Auto           = 2048,
        SpunOut        = 4096,
        AutoPilot      = 8192,
        Perfect        = 16416,
        Cinema         = 4194304,
        TargetPractice = 8388608,
        ScoreV2        = 536870912,
    }

    public enum Ruleset {

        Standard,
        Taiko,
        CatchTheBeat,
        Mania,
    }

    public enum RankedStatus {

        Unknown,
        Unsubmitted,
        Graveyard,   // This can be Pending or WIP too
        Unused,      // No clue
        Ranked,
        Approved,
        Qualified,
        Loved
    }

    public enum ReplayKeyStatus {

        None  = 0,
        M1    = 1,
        M2    = 2,
        K1    = 5,
        K2    = 10,
        Smoke = 16
    }
}
