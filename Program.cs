using System;
using System.Linq;

namespace Ball_sorting_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            for(var i=1;i<=10;i++)
            {
                // Init.
                int cupSize = 4;
                int cupCount = i+2;
                int colorCount = i;
                var riddleCreator = new RiddleCreator(cupSize, cupCount, colorCount);
                var riddleSolver = new RiddleSolver(cupSize, cupCount, colorCount);
                var fileWriter = new FileWriter(@"C:\Ball_sorting_puzzle_creator\Ball-sorting-puzzle\generated\");

                // Workflow steps.
                var randomRiddle = riddleCreator.Create();
                ConsolePrinter.Print(randomRiddle);
                var gameTree = riddleSolver.Solve(randomRiddle);
                if(gameTree.Solutions.Any())
                {
                    //ConsolePrinter.Print(gameTree);
                    fileWriter.WriteToJson(i, gameTree);
                    Console.WriteLine(i);
                }
                else
                {
                    i--;
                }
            }
        }
    }
}
