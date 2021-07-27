using System;

namespace Ball_sorting_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
                // Solve with constraint search.
                // count of possible solutions
                // depth

                var riddleCreator = new RiddleCreator(4,3,2);
                var statistics = riddleCreator.Create();
                //statistics.Dump();
                QualityAssurance.Check(statistics);
        }
    }
}
