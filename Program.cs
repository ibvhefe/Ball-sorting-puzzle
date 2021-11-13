using System;

namespace Ball_sorting_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init.
            byte cupSize = 4;
            byte cupCount = 4;
            byte colorCount = 3;
            var riddleCreator = new RiddleCreator(cupSize, cupCount, colorCount);
            var riddleSolver = new RiddleSolver(cupSize, cupCount, colorCount);

            // Workflow steps.
            var randomRiddle = riddleCreator.Create();
            var gameTree = riddleSolver.Solve(randomRiddle);  
            ConsolePrinter.Print(gameTree);
            FileWriter.WriteToJson(@"C:\Ball_sorting_puzzle_creator\Ball-sorting-puzzle\generated\1.json", gameTree);
        }
    }
}
