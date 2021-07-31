using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ball_sorting_puzzle;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class GameTreeCorrectnessTests
    {
        [TestMethod]
        public void NoDuplicateSolutions()
        {
            byte cupSize = 4;
            byte cupCount = 3;
            byte colorCount = 2;
            var riddleCreator = new RiddleCreator(cupSize, cupCount, colorCount);
            var riddleSolver = new RiddleSolver(cupSize, cupCount, colorCount);

            // Workflow steps.
            var randomRiddle = riddleCreator.Create();
            var gameTree = riddleSolver.Solve(randomRiddle);  

            var count = gameTree.Solutions.Count;
		    var distinctCount = gameTree.Solutions.Distinct(new SolutionComparer()).Count();
            Assert.IsTrue(count == distinctCount);
        }
    }
}
