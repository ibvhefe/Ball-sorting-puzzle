using System;

namespace Ball_sorting_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init.
            byte cupSize = 4;
            byte cupCount = 3;
            byte colorCount = 2;
            var riddleCreator = new RiddleCreator(cupSize, cupCount, colorCount);
            var riddleSolver = new RiddleSolver(cupSize, cupCount, colorCount);

            // Workflow steps.
            var randomRiddle = riddleCreator.Create();
            var gameTree = riddleSolver.Solve(randomRiddle);  
            ConsolePrinter.Print(gameTree);
        }
    }
}
