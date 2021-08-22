using System.Collections.Generic;
using System;
using System.Linq;

public class RiddleSolver : RiddleBase
{
	List<List<SolvingStep>> allSolutions;
	HashSet<SolvingStep> visitedNodes;
	HashSet<SolvingStep> deadendNodes;

	public RiddleSolver(byte cupSize, byte cupCount, byte colorCount) : base(cupSize,cupCount,colorCount)
	{
		this.visitedNodes = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
		this.allSolutions = new List<List<SolvingStep>>();
		this.deadendNodes = new HashSet<SolvingStep>(new BoardComparer(this.cupCount, this.cupSize));
	}

	public GameTreeInfo Solve(byte[,] cups)
	{
		var currentStep = new SolvingStep()
		{
			Board = cups,
			LastMove = new Move(){From=0,To=0}
		};
		var solution = new List<SolvingStep>();
		SolveInternal(solution, currentStep);
		var solutionNodes = CreateHashMap(this.allSolutions);
		var deadendNodeGroups = CollectDeadendNodes(solutionNodes, this.visitedNodes);

		return new GameTreeInfo()
		{
			NodeCount=this.visitedNodes.Count, 
			Solutions = this.allSolutions, 
			SolutionNodeCount = solutionNodes.Count,
			DeadendNodeGroups = deadendNodeGroups,
			DeadendNodeCount = deadendNodeGroups.Sum(g=>g.Count),
			Riddle=cups
		};
	}

	private List<HashSet<SolvingStep>> CollectDeadendNodes(HashSet<SolvingStep> solutionNodes, HashSet<SolvingStep> allNodes)
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

		return deadendNodeGroups;
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

	private void SolveInternal(List<SolvingStep> currentSolution, SolvingStep currentStep)
	{		
		if (IsGoalReached(currentStep.Board))
		{
			currentSolution.Insert(0, currentStep);
			this.allSolutions.Add(Clone(currentSolution));
			visitedNodes.Add(currentStep);
			return;
		}
			
		// Avoid infinite loops.
		if (visitedNodes.Any(step => AreEqual(step.Board, currentStep.Board)))
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
				if (IsMovePossible(currentStep.Board, from, to, fromColor, toColor))
				{
					var nextStep = CreateNextStep(currentStep, from, to);
					SolveInternal(currentSolution, nextStep);
					currentSolution.Remove(nextStep);
				}
			}
		}
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
		clone.LastMove = new Move() { From = from, To = to };
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
			LastMove = step.LastMove,
			NodeType = step.NodeType
		};
	}

	public List<SolvingStep> Clone(List<SolvingStep> steps)
		{
			var clonedList = new List<SolvingStep>();
			foreach(var step in steps)
			{
				clonedList.Add(Clone(step));
			}
			
			return clonedList;
		}

	private byte[,] Clone(byte[,] array)
	{
		var clone = new byte[cupCount,cupSize];
		for(var i = 0; i<=cupCount-1; i++)
		{
			for(var j=0; j<=cupSize-1; j++)
			{
				clone[i,j] = array[i,j];
			}
		}
		return clone;
	}
}