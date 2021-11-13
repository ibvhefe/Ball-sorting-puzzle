using System;
using System.Linq;

namespace Ball_sorting_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            for(var i=0;i<=10;i++)
            {
                // Init.
                byte cupSize = 4;
                byte cupCount = 4;
                byte colorCount = 3;
                var riddleCreator = new RiddleCreator(cupSize, cupCount, colorCount);
                var riddleSolver = new RiddleSolver(cupSize, cupCount, colorCount);
                var fileWriter = new FileWriter(@"C:\Ball_sorting_puzzle_creator\Ball-sorting-puzzle\generated\");

                // Workflow steps.
                var randomRiddle = riddleCreator.Create();
                var gameTree = riddleSolver.Solve(randomRiddle);  
                if(gameTree.Solutions.Any())
                {
                    ConsolePrinter.Print(gameTree);
                    fileWriter.WriteToJson(i, gameTree);
                }
                else
                {
                    i--;
                }
            }
        }
    }
}
