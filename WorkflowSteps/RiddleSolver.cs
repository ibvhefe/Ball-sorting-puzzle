using System.Collections.Generic;
using System;
using System.Linq;

public class RiddleSolver : RiddleBase
{
	List<List<SolvingStep>> allSolutions;
	List<byte[,]> visitedNodes;
	List<byte[,]> deadendNodes;

	public RiddleSolver(byte cupSize, byte cupCount, byte colorCount) : base(cupSize,cupCount,colorCount)
	{
		this.visitedNodes = new List<byte[,]>();
		this.allSolutions = new List<List<SolvingStep>>();
		this.deadendNodes = new List<byte[,]>();
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
		
		return new GameTreeInfo()
		{
			NodeCount=this.visitedNodes.Count, 
			Solutions = this.allSolutions, 
			SolutionNodeCount = GetSolutionNodeCount(this.allSolutions), 
			Riddle=cups
		};
	}

	private Boolean SolveInternal(List<SolvingStep> currentSolution, SolvingStep currentStep)
	{		
		if (IsGoalReached(currentStep.Board))
		{
			currentSolution.Insert(0, currentStep);
			this.allSolutions.Add(Clone(currentSolution));
			visitedNodes.Add(currentStep.Board);
			return true;
		}
			
		// Avoid infinite loops.
		if (visitedNodes.Any(step => AreEqual(step, currentStep.Board)))
		{
		  return allSolutions.SelectMany(s => s ).Contains(step);
		}
		
		visitedNodes.Add(currentStep.Board);
		currentSolution.Insert(0, currentStep);
		
		for(byte from=0;from<=cupCount-1;from++)
		{
			var fromColor = GetUpmostColor(currentStep.Board,from);
			if(fromColor==0)
			{
				continue;
			}
			
			var isDeadendNode = false;
			for(byte to=0;to<=cupCount-1;to++)
			{
				var toColor = GetUpmostColor(currentStep.Board, to);
				if (IsMovePossible(currentStep.Board, from, to, fromColor, toColor))
				{
					var nextStep = CreateNextStep(currentStep, from, to);
					isDeadendNode = isDeadendNode && SolveInternal(currentSolution, nextStep);
					currentSolution.Remove(nextStep);
				}
			}
			return isDeadendNode;
		}
	}

	private int GetSolutionNodeCount(List<List<SolvingStep>> solutions)
	{
		return solutions
						.SelectMany(sol => sol)
						.Select(s => s.Board)
						.Distinct(new BoardComparer(this.cupCount, this.cupSize))
						.Count();
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
			LastMove = step.LastMove
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