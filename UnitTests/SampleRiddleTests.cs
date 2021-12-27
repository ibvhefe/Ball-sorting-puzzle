using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class SampleRiddleTests : TestBase
    {
        [TestMethod]
        public void OnlyOneStepAllowed()
        {
            var riddle = 
@"
0 0 0 0
0 0 0 1
0 0 0 1
0 1 0 1
";

            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(1, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void OnlyOneSolutionAllowed()
        {           
            var riddle =
@"
0 0 0 0
0 0 0 0
0 0 0 0
1 1 1 1
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(1, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void NoSolutions()
        {
            var riddle =
@"
3 0 0 3
2 0 2 3
2 0 2 3
1 1 1 1
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(0, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void WrongSolutionPossible()
        {
            var riddle =
@"
0 0 1 0
1 1 3 0
3 2 3 2
1 2 3 2
";
            var gameTree = SolveRiddle(riddle);
            var count = gameTree.Solutions.Count;
		    var distinctCount = gameTree.Solutions.Distinct(new SolutionComparer()).Count();
            Assert.IsTrue(count == distinctCount);

            Assert.AreEqual(4, gameTree.Solutions.Count);
            Assert.AreEqual(3, gameTree.DeadendNodeCount);
            Assert.AreEqual(2, gameTree.DeadendNodeGroups.Count);
        }

        [TestMethod]
        public void InfiniteLoops()
        {
            var riddle =
@"
0 0 0 0
1 0 0 1
2 2 0 2
2 1 0 1
";
            var sw = new Stopwatch();
            sw.Start();
            var gameTree = SolveRiddle(riddle);
            sw.Stop();
            var actual = sw.Elapsed;
            Assert.AreEqual(476327, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void DifficultTwoColorSample()
        {
            var riddle =
@"
0 2 0
0 1 0
2 2 2
1 1 1
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(10, gameTree.ThreeStarLimit);
            Assert.AreEqual(11, gameTree.TwoStarLimit);
            Assert.AreEqual(12, gameTree.OneStarLimit);
        }

        [TestMethod]
        public void DifficultFourColorSample()
        {
var riddle =
@"
0 0 2 1
0 1 1 3
0 2 1 2
2 3 3 3
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(16, gameTree.ThreeStarLimit);
            Assert.AreEqual(17, gameTree.TwoStarLimit);
            Assert.AreEqual(18, gameTree.OneStarLimit);
        }

        [TestMethod]
        public void DifficultFiveColorSample()
        {
var riddle =
@"
1 0 0 4 0 3
2 4 5 2 0 1
4 2 4 3 3 5
1 3 5 5 1 2
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(26, gameTree.ThreeStarLimit);
            Assert.AreEqual(28, gameTree.TwoStarLimit);
            Assert.AreEqual(30, gameTree.OneStarLimit);
        }



        [TestMethod]
        public void ManyDeadendNodes()
        {
            var riddle =
@"
0 0 0 0
1 1 1 1
3 3 2 2
3 3 2 2
";
            var gameTree = SolveRiddle(riddle);
            var count = gameTree.Solutions.Count;
		    var distinctCount = gameTree.Solutions.Distinct(new SolutionComparer()).Count();
            Assert.IsTrue(count == distinctCount);
            // todo what the fuck(?)
            Assert.AreEqual(140, gameTree.Solutions.Count);
            Assert.AreEqual(3, gameTree.DeadendNodeCount);
        }

        [TestMethod]
        public void TwoDeadendNodesThatLeadToTheSameLeaf()
        {
            var riddle =
@"
2 1 0
2 2 0
2 1 0
1 1 0
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(1, gameTree.Solutions.Count);
            Assert.AreEqual(2, gameTree.DeadendNodeCount);
            Assert.AreEqual(1, gameTree.DeadendNodeGroups.Count);
        }

        [TestMethod]
        public void BestSolutionMustBeOptimal()
        {
            var riddle =
@"
2 0 1 0
1 2 3 0
2 1 2 0
3 3 1 3
";
            var gameTree = SolveRiddle(riddle);
            Assert.AreEqual(15, gameTree.Solutions.Select(s=>s.Count).Min()-1);
        }
    }
}