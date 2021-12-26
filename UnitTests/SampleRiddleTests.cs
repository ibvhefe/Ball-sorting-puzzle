using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class SampleRiddleTests
    {
        [TestMethod]
        public void OnlyOneStepAllowed()
        {
            var node = new byte[4,4];
            node[0,0]=0;
            node[0,1]=0;
            node[0,2]=0;
            node[0,3]=0;
            node[1,0]=1;
            node[1,1]=0;
            node[1,2]=0;
            node[1,3]=0;
            node[2,0]=0;
            node[2,1]=0;
            node[2,2]=0;
            node[2,3]=0;
            node[3,0]=1;
            node[3,1]=1;
            node[3,2]=1;
            node[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 1);
            var gameTree = riddleSolver.Solve(node);
            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);
            Assert.AreEqual(1, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void OnlyOneSolutionAllowed()
        {
            var node = new byte[4,4];
            node[0,0]=1;
            node[0,1]=0;
            node[0,2]=0;
            node[0,3]=0;
            node[1,0]=1;
            node[1,1]=0;
            node[1,2]=0;
            node[1,3]=0;
            node[2,0]=1;
            node[2,1]=0;
            node[2,2]=0;
            node[2,3]=0;
            node[3,0]=1;
            node[3,1]=0;
            node[3,2]=0;
            node[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 1);
            var gameTree = riddleSolver.Solve(node);
            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);
            Assert.AreEqual(1, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void NoSolutions()
        {
            var node = new byte[4,4];
            node[0,0]=1;
            node[0,1]=2;
            node[0,2]=2;
            node[0,3]=3;
            node[1,0]=1;
            node[1,1]=0;
            node[1,2]=0;
            node[1,3]=0;
            node[2,0]=1;
            node[2,1]=2;
            node[2,2]=2;
            node[2,3]=0;
            node[3,0]=3;
            node[3,1]=3;
            node[3,2]=3;
            node[3,3]=1;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(node);
            Assert.AreEqual(0, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void WrongSolutionPossible()
        {
            var node = new byte[4,4];
            node[0,0]=1;
            node[0,1]=3;
            node[0,2]=1;
            node[0,3]=0;

            node[1,0]=2;
            node[1,1]=2;
            node[1,2]=1;
            node[1,3]=0;

            node[2,0]=3;
            node[2,1]=3;
            node[2,2]=3;
            node[2,3]=1;

            node[3,0]=2;
            node[3,1]=2;
            node[3,2]=0;
            node[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(node);
            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);

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
            var node = new byte[4,4];
            node[0,0]=2;
            node[0,1]=2;
            node[0,2]=1;
            node[0,3]=0;

            node[1,0]=1;
            node[1,1]=2;
            node[1,2]=0;
            node[1,3]=0;

            node[2,0]=0;
            node[2,1]=0;
            node[2,2]=0;
            node[2,3]=0;

            node[3,0]=1;
            node[3,1]=2;
            node[3,2]=1;
            node[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 2);
            var sw = new Stopwatch();
            sw.Start();
            var gameTree = riddleSolver.Solve(node);
            sw.Stop();
            var actual = sw.Elapsed;
            //ConsolePrinter.SetDebugMode();
            //ConsolePrinter.Print(gameTree);
            Assert.AreEqual(476327, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void DifficultFourColorSample()
        {
            var node = new byte[4,4];
            node[0,0]=2;
            node[0,1]=0;
            node[0,2]=0;
            node[0,3]=0;

            node[1,0]=3;
            node[1,1]=2;
            node[1,2]=1;
            node[1,3]=0;

            node[2,0]=3;
            node[2,1]=1;
            node[2,2]=1;
            node[2,3]=2;

            node[3,0]=3;
            node[3,1]=2;
            node[3,2]=3;
            node[3,3]=1;

            ConsolePrinter.SetDebugMode();
            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(node);
            ConsolePrinter.Print(gameTree);

            Assert.AreEqual(16, gameTree.ThreeStarLimit);
            Assert.AreEqual(17, gameTree.TwoStarLimit);
            Assert.AreEqual(18, gameTree.OneStarLimit);
        }

        [TestMethod]
        public void DifficultFiveColorSample()
        {
            var node = new byte[6,4];
            node[0,0]=1;
            node[0,1]=4;
            node[0,2]=2;
            node[0,3]=1;

            node[1,0]=3;
            node[1,1]=2;
            node[1,2]=4;
            node[1,3]=0;

            node[2,0]=5;
            node[2,1]=4;
            node[2,2]=5;
            node[2,3]=0;

            node[3,0]=5;
            node[3,1]=3;
            node[3,2]=2;
            node[3,3]=4;

            node[4,0]=1;
            node[4,1]=3;
            node[4,2]=0;
            node[4,3]=0;
            
            node[5,0]=2;
            node[5,1]=5;
            node[5,2]=1;
            node[5,3]=3;

            ConsolePrinter.SetDebugMode();
            var riddleSolver = new RiddleSolver(4, 6, 5);
            var gameTree = riddleSolver.Solve(node);
            ConsolePrinter.Print(gameTree);

            Assert.AreEqual(26, gameTree.ThreeStarLimit);
            Assert.AreEqual(28, gameTree.TwoStarLimit);
            Assert.AreEqual(30, gameTree.OneStarLimit);
        }



        [TestMethod]
        public void ManyDeadendNodes()
        {
            var node = new byte[4,4];
            node[0,0]=3;
            node[0,1]=3;
            node[0,2]=1;
            node[0,3]=0;

            node[1,0]=3;
            node[1,1]=3;
            node[1,2]=1;
            node[1,3]=0;
            
            node[2,0]=2;
            node[2,1]=2;
            node[2,2]=1;
            node[2,3]=0;

            node[3,0]=2;
            node[3,1]=2;
            node[3,2]=1;
            node[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(node);
            //ConsolePrinter.SetDebugMode();
            //ConsolePrinter.Print(gameTree.DeadendNodeGroups.SelectMany(n=>n).Select(i=>i.Board).ToList());

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
            var node = new byte[3,4];
            node[0,0]=1;
            node[0,1]=2;
            node[0,2]=2;
            node[0,3]=2;
            node[1,0]=1;
            node[1,1]=1;
            node[1,2]=2;
            node[1,3]=1;
            node[2,0]=0;
            node[2,1]=0;
            node[2,2]=0;
            node[2,3]=0;

            var riddleSolver = new RiddleSolver(4, 3, 2);
            var gameTree = riddleSolver.Solve(node);

            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);
            Assert.AreEqual(1, gameTree.Solutions.Count);
            Assert.AreEqual(2, gameTree.DeadendNodeCount);
            Assert.AreEqual(1, gameTree.DeadendNodeGroups.Count);
        }

        [TestMethod]
        public void CanDeadendNodeConnectToSet()
        {
            var riddle = new byte[4,4];
            riddle[0,0]=3;
            riddle[0,1]=3;
            riddle[0,2]=1;
            riddle[0,3]=0;
            riddle[1,0]=3;
            riddle[1,1]=3;
            riddle[1,2]=1;
            riddle[1,3]=0;
            riddle[2,0]=2;
            riddle[2,1]=2;
            riddle[2,2]=1;
            riddle[2,3]=0;
            riddle[3,0]=2;
            riddle[3,1]=2;
            riddle[3,2]=1;
            riddle[3,3]=0;

            var node = new byte[4,4];
            node[0,0]=3;
            node[0,1]=3;
            node[0,2]=1;
            node[0,3]=1;
            node[1,0]=3;
            node[1,1]=3;
            node[1,2]=1;
            node[1,3]=0;
            node[2,0]=2;
            node[2,1]=2;
            node[2,2]=1;
            node[2,3]=0;
            node[3,0]=2;
            node[3,1]=2;
            node[3,2]=0;
            node[3,3]=0;

            var solver = new RiddleSolver(4,4,3);
            var info = solver.Solve(node);
            var distinct = solver.CreateHashMap(info.Solutions);
            var actual = solver.IsConnected(new SolvingStep{Board=node},distinct);

            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(riddle);
            ConsolePrinter.Print(node);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void BestSolutionMustBeOptimal()
        {
            var node = new byte[4,4];
            node[0,0]=3;
            node[0,1]=2;
            node[0,2]=1;
            node[0,3]=2;

            node[1,0]=3;
            node[1,1]=1;
            node[1,2]=2;
            node[1,3]=0;

            node[2,0]=1;
            node[2,1]=2;
            node[2,2]=3;
            node[2,3]=1;

            node[3,0]=3;
            node[3,1]=0;
            node[3,2]=0;
            node[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(node);

            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);
            Assert.AreEqual(15, gameTree.Solutions.Select(s=>s.Count).Min()-1);
        }
    }
}