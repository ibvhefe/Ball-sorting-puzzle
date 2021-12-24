using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ball_sorting_puzzle;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class RiddleSolverTests
    {
        [TestMethod]
        public void HasRightCopy()
        {
            var node = new byte[4,4];
            node[0,0]=2;
            node[0,1]=2;
            node[0,2]=2;
            node[0,3]=2;

            node[1,0]=1;
            node[1,1]=2;
            node[1,2]=1;
            node[1,3]=3;

            node[2,0]=1;
            node[2,1]=2;
            node[2,2]=1;
            node[2,3]=3;

            node[3,0]=3;
            node[3,1]=3;
            node[3,2]=0;
            node[3,3]=0;

            var solver = new RiddleSolver(4,4,3);
            Assert.IsFalse(solver.HasCopyToTheRight(node,0));
            Assert.IsTrue(solver.HasCopyToTheRight(node,1));
            Assert.IsFalse(solver.HasCopyToTheRight(node,2));
            Assert.IsFalse(solver.HasCopyToTheRight(node,3));
        }

        [TestMethod]
        public void GetCollectorTubePositionIsFoundForCompleteCup()
        {
            var node = new byte[4,4];
            node[0,0]=1;
            node[0,1]=1;
            node[0,2]=1;
            node[0,3]=1;

            node[1,0]=2;
            node[1,1]=2;
            node[1,2]=2;
            node[1,3]=2;

            node[2,0]=3;
            node[2,1]=3;
            node[2,2]=0;
            node[2,3]=0;

            node[3,0]=3;
            node[3,1]=3;
            node[3,2]=0;
            node[3,3]=0;

            var solver = new RiddleSolver(4,4,3);
            Assert.AreEqual(0, solver.GetBestCollectorTubePosition(node,1));
            Assert.AreEqual(1, solver.GetBestCollectorTubePosition(node,2));
        }

        [TestMethod]
        public void GetCollectorTubePositionSelectsBiggestTube()
        {
            var node = new byte[4,4];
            node[0,0]=1;
            node[0,1]=0;
            node[0,2]=0;
            node[0,3]=0;

            node[1,0]=1;
            node[1,1]=1;
            node[1,2]=0;
            node[1,3]=0;

            node[2,0]=2;
            node[2,1]=1;
            node[2,2]=0;
            node[2,3]=0;

            node[3,0]=3;
            node[3,1]=3;
            node[3,2]=3;
            node[3,3]=3;

            var solver = new RiddleSolver(4,4,3);
            Assert.AreEqual(1, solver.GetBestCollectorTubePosition(node,1));
        }

        [TestMethod]
        public void GetCollectorTubeSelectsMostLeftTubeForEqualSizes()
        {
            var node = new byte[4,4];
            node[0,0]=0;
            node[0,1]=0;
            node[0,2]=0;
            node[0,3]=0;

            node[1,0]=1;
            node[1,1]=1;
            node[1,2]=0;
            node[1,3]=0;

            node[2,0]=3;
            node[2,1]=3;
            node[2,2]=3;
            node[2,3]=3;

            node[3,0]=1;
            node[3,1]=1;
            node[3,2]=0;
            node[3,3]=0;

            var solver = new RiddleSolver(4,4,3);
            Assert.AreEqual(1, solver.GetBestCollectorTubePosition(node,1));
        }
    }
}