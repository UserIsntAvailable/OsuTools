using OsuTools.Models;

namespace OsuTools.Interfaces {
    public interface IBeatmapInfo {

        /* I don't want that the people have access to Hits
         so => {
         I wanna that be internal but I have access problems, 
         I change struct to class and create a ctor internal, 
         I give the public access to Hits, 
         I create a propfull in Score }
         
         Change Helper.GetAcc( Hits h ) if I change of opinion 
         Create Accuracy prop on Hits?*/

        // Change all the ToString() Methods

        // Create an IScoreInfo

        GameMode Mode { get; set; }
        string Hash { get; set; }
        int MaxCombo { get; set; }
    }
}
