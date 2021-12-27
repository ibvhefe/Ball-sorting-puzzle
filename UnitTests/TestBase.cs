using System.Linq;
using System;

namespace UnitTests
{
    public class TestBase
    {
        protected GameTreeInfo SolveRiddle(string riddle)
        {
            var riddleSolver = CreateSolver(riddle);
            var problem = ReadFormattedString(riddle);
            return riddleSolver.Solve(problem);
        }

        protected RiddleSolver CreateSolver(string riddle)
        {
            var lines = riddle.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.Reverse();
            var cupSize = lines.Count();
            var cupCount = lines[0].Split(" ").Count();
            var colorCount = 0;
            for (var row = 0; row<cupSize; row++)
            {
                var entries = lines[row].Split(' ');
                for(var column = 0; column<cupCount; column++)
                {
                    var entry = byte.Parse(entries[column]);
                    if(entry>colorCount)
                    {
                        colorCount = entry;
                    }
                }
            }

            var riddleSolver = new RiddleSolver(cupSize, cupCount, colorCount);
            return riddleSolver;
        }

        protected byte[,] ReadFormattedString(string riddle)
        {
            var lines = riddle.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.Reverse();
            var cupSize = lines.Count();
            var cupCount = lines[0].Split(" ").Count();
            var colorCount = 0;
            var problem = new byte[cupCount,cupSize];
            for (var row = 0; row<cupSize; row++)
            {
                var entries = lines[row].Split(' ');
                for(var column = 0; column<cupCount; column++)
                {
                    var entry = byte.Parse(entries[column]);
                    problem[column,row] = entry;
                    if(entry>colorCount)
                    {
                        colorCount = entry;
                    }
                }
            }
            return problem;
        }
    }
}