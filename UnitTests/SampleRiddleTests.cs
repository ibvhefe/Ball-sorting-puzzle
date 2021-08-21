using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ball_sorting_puzzle;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class SampleRiddleTests
    {
        [TestMethod]
        public void NoSolutions()
        {
            var unsolveableRiddle = new byte[4,4];
            unsolveableRiddle[0,0]=1;
            unsolveableRiddle[0,1]=2;
            unsolveableRiddle[0,2]=2;
            unsolveableRiddle[0,3]=3;
            unsolveableRiddle[1,0]=1;
            unsolveableRiddle[1,1]=0;
            unsolveableRiddle[1,2]=0;
            unsolveableRiddle[1,3]=0;
            unsolveableRiddle[2,0]=1;
            unsolveableRiddle[2,1]=2;
            unsolveableRiddle[2,2]=2;
            unsolveableRiddle[2,3]=0;
            unsolveableRiddle[3,0]=3;
            unsolveableRiddle[3,1]=3;
            unsolveableRiddle[3,2]=3;
            unsolveableRiddle[3,3]=1;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(unsolveableRiddle);
            Assert.AreEqual(0, gameTree.Solutions.Count);
        }

        [TestMethod]
        public void WrongSolutionPossible()
        {
            var unsolveableRiddle = new byte[4,4];
            unsolveableRiddle[0,0]=1;
            unsolveableRiddle[0,1]=3;
            unsolveableRiddle[0,2]=1;
            unsolveableRiddle[0,3]=0;
            unsolveableRiddle[1,0]=2;
            unsolveableRiddle[1,1]=2;
            unsolveableRiddle[1,2]=1;
            unsolveableRiddle[1,3]=0;
            unsolveableRiddle[2,0]=3;
            unsolveableRiddle[2,1]=3;
            unsolveableRiddle[2,2]=3;
            unsolveableRiddle[2,3]=1;
            unsolveableRiddle[3,0]=2;
            unsolveableRiddle[3,1]=2;
            unsolveableRiddle[3,2]=0;
            unsolveableRiddle[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(unsolveableRiddle);
            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);

            Assert.AreEqual(4, gameTree.Solutions.Count);
            Assert.AreEqual(1, gameTree.DeadendNodeCount);
        }

        [TestMethod]
        public void ManyDeadendNodes()
        {
            var unsolveableRiddle = new byte[4,4];
            unsolveableRiddle[0,0]=3;
            unsolveableRiddle[0,1]=3;
            unsolveableRiddle[0,2]=1;
            unsolveableRiddle[0,3]=0;
            unsolveableRiddle[1,0]=3;
            unsolveableRiddle[1,1]=3;
            unsolveableRiddle[1,2]=1;
            unsolveableRiddle[1,3]=0;
            unsolveableRiddle[2,0]=2;
            unsolveableRiddle[2,1]=2;
            unsolveableRiddle[2,2]=1;
            unsolveableRiddle[2,3]=0;
            unsolveableRiddle[3,0]=2;
            unsolveableRiddle[3,1]=2;
            unsolveableRiddle[3,2]=1;
            unsolveableRiddle[3,3]=0;

            var riddleSolver = new RiddleSolver(4, 4, 3);
            var gameTree = riddleSolver.Solve(unsolveableRiddle);
            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);

            Assert.AreEqual(8, gameTree.Solutions.Count);
            Assert.AreEqual(4, gameTree.DeadendNodeCount);
        }

        [TestMethod]
        public void TwoDeadendNodesThatLeadToTheSameLeaf()
        {
            var unsolveableRiddle = new byte[3,4];
            unsolveableRiddle[0,0]=1;
            unsolveableRiddle[0,1]=2;
            unsolveableRiddle[0,2]=2;
            unsolveableRiddle[0,3]=2;
            unsolveableRiddle[1,0]=1;
            unsolveableRiddle[1,1]=1;
            unsolveableRiddle[1,2]=2;
            unsolveableRiddle[1,3]=1;
            unsolveableRiddle[2,0]=0;
            unsolveableRiddle[2,1]=0;
            unsolveableRiddle[2,2]=0;
            unsolveableRiddle[2,3]=0;

            var riddleSolver = new RiddleSolver(4, 3, 2);
            var gameTree = riddleSolver.Solve(unsolveableRiddle);

            ConsolePrinter.SetDebugMode();
            ConsolePrinter.Print(gameTree);
            //Assert.AreEqual(0, gameTree.Solutions.Count);
        }
    }
}