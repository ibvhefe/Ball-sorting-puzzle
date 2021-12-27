using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class RiddleSolverTests : TestBase
    {
         [TestMethod]
        public void CanDeadendNodeConnectToSet()
        {
            var riddle = 
@"
1 0 0 0
1 1 1 0
3 3 2 2
3 3 2 2
";
            var gameTree = SolveRiddle(riddle);
            var solver = CreateSolver(riddle);
            var board = ReadFormattedString(riddle);
            var distinct = solver.CreateHashMap(gameTree.Solutions);
            var actual = solver.IsConnected(new SolvingStep{Board=board},distinct);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void HasRightCopy()
        {
            var riddle = 
@"
2 3 3 0
2 1 1 0
2 2 2 3
2 1 1 3
";
            var solver = CreateSolver(riddle);
            var board = ReadFormattedString(riddle);
            Assert.IsFalse(solver.HasCopyToTheRight(board,0));
            Assert.IsTrue(solver.HasCopyToTheRight(board,1));
            Assert.IsFalse(solver.HasCopyToTheRight(board,2));
            Assert.IsFalse(solver.HasCopyToTheRight(board,3));
        }

        [TestMethod]
        public void GetCollectorTubePositionIsFoundForCompleteCup()
        {
            var riddle = 
@"
1 2 0 0
1 2 0 0
1 2 3 3
1 2 3 3
";
            var solver = CreateSolver(riddle);
            var board = ReadFormattedString(riddle);
            Assert.AreEqual(0, solver.GetBestCollectorTubePosition(board,1));
            Assert.AreEqual(1, solver.GetBestCollectorTubePosition(board,2));
        }

        [TestMethod]
        public void GetCollectorTubePositionSelectsBiggestTube()
        {
            var riddle = 
@"
0 0 0 3
0 0 0 3
0 1 1 3
1 1 2 3
";
            var solver = CreateSolver(riddle);
            var board = ReadFormattedString(riddle);
            Assert.AreEqual(1, solver.GetBestCollectorTubePosition(board,1));
        }

        [TestMethod]
        public void GetCollectorTubeSelectsMostLeftTubeForEqualSizes()
        {
            var riddle = 
@"
0 0 3 0
0 0 3 0
0 1 3 1
0 1 3 1
";
            var solver = CreateSolver(riddle);
            var board = ReadFormattedString(riddle);
            Assert.AreEqual(1, solver.GetBestCollectorTubePosition(board,1));
        }
    }
}