using System.Collections.Generic;
using System;
using System.Linq;

public class RiddleSolver : RiddleBase
{
	List<List<SolvingStep>> allSolutions;
	HashSet<SolvingStep> visitedNodes;
	HashSet<SolvingStep> deadendNodes;

	Boolean solutionFound = false;

	public RiddleSolver(int cupSize, int cupCount, int colorCount) : base(cupSize,cupCount,colorCount)
	{
		this.visitedNodes = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
		this.allSolutions = new List<List<SolvingStep>>();
		this.deadendNodes = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
	}

	public GameTreeInfo Solve(byte[,] cups, Boolean onlyFindOneSolution=false)
	{
		var currentStep = new SolvingStep()
		{
			Board = cups,
			From = 0,
			To = 0
		};
		var solution = new List<SolvingStep>();
		SolveInternal(solution, currentStep, onlyFindOneSolution);
		var solutionNodes = CreateHashMap(this.allSolutions);
		
		(List<HashSet<SolvingStep>>, HashSet<SolvingStep>) deadendNodeGroups = (new List<HashSet<SolvingStep>>(),new HashSet<SolvingStep>());
		if(onlyFindOneSolution)
		{
			deadendNodeGroups = CollectDeadendNodes(solutionNodes, this.visitedNodes);
		}
		

		var perfectMoveCount = -1;
		var badMoveCount = -1;
		if(this.allSolutions.Any())
		{
			perfectMoveCount = this.allSolutions.Select(s=>s.Count).Min()-1;
			badMoveCount = this.allSolutions.Select(s=>s.Count).Max()-1;
			if(badMoveCount<perfectMoveCount+2)
			{
				badMoveCount = perfectMoveCount+2;
			}
		}
		
		return new GameTreeInfo()
		{
			ColorCount = this.colorCount,
			NodeCount=this.visitedNodes.Count, 
			Solutions = this.allSolutions, 
			SolutionNodeCount = solutionNodes.Count,
			DeadendNodeGroups = deadendNodeGroups.Item1,
			DeadendNodeCount = deadendNodeGroups.Item1.Sum(g=>g.Count),
			CanReachSolutionCount = deadendNodeGroups.Item2.Count,
			CanReachSolutions = deadendNodeGroups.Item2,
			OneStarLimit = badMoveCount,
			TwoStarLimit = badMoveCount+(perfectMoveCount-badMoveCount)/2,
			ThreeStarLimit = perfectMoveCount,
			Riddle=cups
		};
	}

	private (List<HashSet<SolvingStep>>, HashSet<SolvingStep>) CollectDeadendNodes(HashSet<SolvingStep> solutionNodes, HashSet<SolvingStep> allNodes)
	{
		var noSolutionNodes = GetNoSolutionNodes(solutionNodes, allNodes);
		var canReachSolutionNodes = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
		var deadendNodeGroups = new List<HashSet<SolvingStep>>();

		foreach(var n in noSolutionNodes)
		{
			if(IsConnected(n, canReachSolutionNodes))
			{
				canReachSolutionNodes.Add(n);
				continue;
			}

			if(IsConnected(n, solutionNodes))
			{
				canReachSolutionNodes.Add(n);
				continue;
			}

			foreach(var group in deadendNodeGroups)
			{
				if(IsConnected(n, group))
				{
					group.Add(n);
					goto Next;
				}
			}

			var newDeadendGroup = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
			newDeadendGroup.Add(n);
			deadendNodeGroups.Add(newDeadendGroup);

			Next:;
		}

		return (deadendNodeGroups,canReachSolutionNodes);
	}

	private List<SolvingStep> GetNoSolutionNodes(HashSet<SolvingStep> solutionNodes, HashSet<SolvingStep> allNodes)
	{
		return allNodes.Except(solutionNodes, new BoardComparer(this.cupCount, this.cupSize)).ToList();
	}

	public Boolean IsConnected(SolvingStep node, HashSet<SolvingStep> set)
	{
		var visited = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
		return IsConnectedRecursive(node, set, visited);
	}

	private Boolean IsConnectedRecursive(SolvingStep node, HashSet<SolvingStep> set, HashSet<SolvingStep> visited)
	{
		if(set.Contains(node))
		{
			return true;
		}

		if(visited.Contains(node))
		{
			return false;
		}

		visited.Add(node);

		for(byte from=0;from<=cupCount-1;from++)
		{
			var fromColor = GetUpmostColor(node.Board,from);
			if(fromColor==0)
			{
				continue;
			}
			
			for(byte to=0;to<=cupCount-1;to++)
			{
				var toColor = GetUpmostColor(node.Board, to);
				if (IsMovePossible(node.Board, from, to, fromColor, toColor))
				{
					var nextStep = CreateNextStep(node, from, to);
					if(IsConnectedRecursive(nextStep, set, visited))
					{
						return true;
					}
				}
			}
		}

		return false;
	}

    

	private void SolveInternal(List<SolvingStep> currentSolution, SolvingStep currentStep, Boolean onlyFindOneSolution)
	{
		if(onlyFindOneSolution && solutionFound)
		{
			return;
		}

		if (IsGoalReached(currentStep.Board))
		{
			currentSolution.Insert(0, currentStep);
			this.allSolutions.Add(Clone(currentSolution));
			visitedNodes.Add(currentStep);
			solutionFound = true;
			Console.WriteLine("solution found");
			return;
		}
			
		// Avoid infinite loops.
		if (currentSolution.Any(step => AreEqual(step.Board, currentStep.Board)))
		{
		  return;
		}
		
		visitedNodes.Add(currentStep);
		currentSolution.Insert(0, currentStep);
		
		for(byte from=0;from<=cupCount-1;from++)
		{
			var fromColor = GetUpmostColor(currentStep.Board,from);
			if(fromColor==0)
			{
				continue;
			}
			
			for(byte to=0;to<=cupCount-1;to++)
			{
				var toColor = GetUpmostColor(currentStep.Board, to);
				var collectorTubePosition = GetBestCollectorTubePosition(currentStep.Board, fromColor);
				if (IsMovePossible(currentStep.Board, from, to, fromColor, toColor) && 
				    DoesMoveMakeSense(currentStep.Board, from, to, fromColor, toColor, collectorTubePosition))
				{
					if(onlyFindOneSolution && solutionFound)
		            {
			            return;
		            }
					var nextStep = CreateNextStep(currentStep, from, to);
					SolveInternal(currentSolution, nextStep, onlyFindOneSolution);
					currentSolution.Remove(nextStep);
				}
			}
		}
	}

	private Boolean DoesMoveMakeSense(byte[,] cups, byte from, byte to, byte fromColor, byte toColor, int mainCollectorTubePosition)
	{		
		// Do not create a new collector tube, if you already have one.
		if(mainCollectorTubePosition==from && toColor==0)
		{
			return false;
		}
		var sourceIsCollectorTube = IsCollectorTubeOfColor(cups,from,fromColor);
		if(sourceIsCollectorTube && toColor==0)
		{
			return false;
		}

		// Do not feed lesser collector tubes.
		var targetIsCollectorTube = IsCollectorTubeOfColor(cups,to,fromColor);
		if(targetIsCollectorTube && from==mainCollectorTubePosition)
		{
			return false;
		}

		// Do not feed collector tubes, that are not main collectors.
		if(targetIsCollectorTube && to!=mainCollectorTubePosition)
		{
			return false;
		}

		// Do not start a move, if the same move has been done by a left neighbour before.
		if(HasCopyToTheRight(cups, from))
		{
			return false;
		}

        return true;
	}

	internal bool HasCopyToTheRight(byte[,] cups, byte column)
	{
		for(var i=column+1;i<cupCount;i++)
		{
			if(AreEqual(cups,i,column))
			{
				return true;
			}
		}

		return false;
	}

	private bool AreEqual(byte[,] cups, int a, byte b)
	{
		for(var i=0;i<cupSize;i++)
		{
			if(cups[a,i]!=cups[b,i])
			{
				return false;
			}
		}

		return true;
	}

    private bool IsCollectorTubeOfColor(byte[,] cups, byte column, byte color)
	{
		if(cups[column,0]!=color)
		{
			return false;
		}

        for(var row=1;row<cupSize; row++)
		{
			var currentColor = cups[column,row];
			if(currentColor!=color && currentColor!=0)
			{
				return false;
			}
		}
		
		return true;
	}

	internal int GetBestCollectorTubePosition(byte[,] cups, byte fromColor)
	{
		var globalMax=0;
		var collectorTubePosition=-1;
		for(var column=0; column < cupCount; column++)
		{
			var currentMax=0;
			for(var row=0;row<cupSize; row++)
			{
				var currentColor = cups[column,row];
				if(currentColor==fromColor)
				{
					currentMax++;
				}
				else if (currentColor==0)
				{
					// Do nothing.
				}
				else
				{
					currentMax=0;
					break;
				}
			}
			if(currentMax>globalMax)
			{
				globalMax = currentMax;
				collectorTubePosition = column;
			}
		}

		return collectorTubePosition;
	}

	public HashSet<SolvingStep> CreateHashMap(List<List<SolvingStep>> solutions)
	{
		return solutions
						.SelectMany(sol => sol)
						.Distinct(new BoardComparer(this.cupCount, this.cupSize))
						.ToHashSet(new BoardComparer(this.cupCount, this.cupSize));
	}

	private Boolean AreEqual(byte[,] a, byte[,] b)
	{
		for (byte i = 0; i <= cupCount-1; i++)
		{
			for (byte j = 0; j <= cupSize-1; j++)
			{
				if (a[i, j] != b[i, j])
				{
					return false;
				}
			}
		}

		return true;
	}

	private SolvingStep CreateNextStep(SolvingStep currentStep, byte from, byte to)
	{
		var clone = Clone(currentStep);
		clone.From = from;
		clone.To = to;
		var pulledColor = Pull(clone.Board, from);
		Push(clone.Board, pulledColor, to);
		return clone;
	}

	private byte Pull(byte[,] cups, byte from)
	{
		for (var i = cupSize - 1; i >= 0; i--)
		{
			var currentColor = cups[from, i];
			if (currentColor != 0)
			{
				cups[from,i]=0;
				return currentColor;
			}
		}
		
		return 0;
	}

	private Boolean IsMovePossible(byte[,] cups, byte from, byte to, byte fromColor, byte toColor)
	{
		if (to == from)
		{
			return false;
		}
		
		// Source cup is complete
		var allColorsAreEqual = true;
		for(byte i=0; i<=cupSize-1; i++)
		{
			if(cups[from,i]!=fromColor)
			{
				allColorsAreEqual = false;
				break;
			}
		}
		if(allColorsAreEqual)
		{
			return false;
		}
		// n-1 is also complete, maybe n-2, ...
		
		// Target cup is full.
		if(cups[to,cupSize-1]!=0)
		{
			return false;
		}

		if (toColor == 0)
		{
			return true;
		}
		
		return fromColor==toColor;
	}

	private byte GetUpmostColor(byte[,] cups, byte column)
	{
		for (var i = cupSize - 1; i >= 0; i--)
		{
			var currentColor = cups[column, i];
			if (currentColor != 0)
			{
				return currentColor;
			}
		}
		
		// Cup was empty.
		return 0;
	}

	public Boolean IsGoalReached(byte[,] cups)
	{
		for (var i = 0; i <= cupCount - 1; i++)
		{
			var currentColor = cups[i, 0];
			for (var j = 0; j <= cupSize - 1; j++)
			{
				if(cups[i, j] != currentColor)
				{
					return false;
				}
			}
		}
		
		return true;
	}

	private SolvingStep Clone(SolvingStep step)
	{
		return new SolvingStep
		{
			Board = Clone(step.Board),
			From = step.From,
			To = step.To,
		};
	}

	public List<SolvingStep> Clone(List<SolvingStep> steps)
		{	
			/*var clonedList = new List<SolvingStep>(steps.Count);
			foreach(var step in steps)
			{
				clonedList.Add(Clone(step));
			}
			
			return clonedList;*/
			return steps.ToList();
		}

	private byte[,] Clone(byte[,] array)
	{
		var clone = new byte[cupCount,cupSize];
		Buffer.BlockCopy(array, 0, clone, 0, arraySize);
		return clone;
	}
}