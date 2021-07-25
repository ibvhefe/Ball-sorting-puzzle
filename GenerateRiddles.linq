<Query Kind="Program" />

void Main()
{
    // Solve with constraint search.
	// count of possible solutions
	// depth

    var riddleCreator = new RiddleCreator(4,3,2);
    riddleCreator.Create();
}

class RiddleCreator
{
byte cupSize;
byte cupCount;
byte colorCount;
int nodeCount;
List<List<SolvingStep>> allSolutions;

public RiddleCreator(byte cupSize, byte cupCount, byte colorCount)
{
    this.cupSize = cupSize;
    this.cupCount = cupCount;
    this.colorCount = colorCount;
    this.nodeCount = 0;
    this.allSolutions = new List<List<SolvingStep>>();
}

public CreationStatistics Create()
{
    var cups = new byte[cupCount, cupSize];
	DistributeRandomly(cups);
	//cups[0,0]=0;
	//cups[0,1]=0;
	//cups[0,2]=0;
	//cups[0, 3] = 0;
	//cups[1, 0] = 2;
	//cups[1, 1] = 1;
	//cups[1, 2] = 1;
	//cups[1, 3] = 1;
	//cups[2, 0] = 1;
	//cups[2, 1] = 2;
	//cups[2, 2] = 2;
	//cups[2, 3] = 2;
	//cups.Dump();
	
	var currentStep = new SolvingStep()
	{
		Board = cups,
		MoveCount = 0,
		LastMove = new Move(){From=0,To=0}
	};
	var solution = new List<SolvingStep>();
	SolveInternal(solution, currentStep);
	solution.Dump();
	
	return new CreationStatistics(){NodeCount=this.nodeCount};
}

private Boolean SolveInternal(List<SolvingStep> currentSolution, SolvingStep currentStep)
{
    // Avoid infinite loops.
	if (currentSolution.Any(step => AreEqual(step.Board, currentStep.Board)))
	{
		return;
	}
	
	currentSolution.Insert(0, currentStep);
    this.nodeCount++;
    
	if (IsGoalReached(currentStep.Board))
	{
	    this.allSolutions.Add(Clone(currentSolution));
		return;
	}
	
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
			}
		}
	}
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
	Push(clone.Board, to, pulledColor);
	return clone;
}

private void Push(byte[,] cups, byte to, byte toColor)
{
	for (var i = 0; i <= cupSize-1; i++)
	{
		var currentColor = cups[to, i];
		if (currentColor == 0)
		{
			cups[to, i] = toColor;
			return;
		}
	}
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

private Boolean IsGoalReached(byte[,] cups)
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
		Board = Clone(step.Board);
		LastMove = step.LastMove
	};
}

private List<SolvingStep> Clone(List<SolvingStep> steps)
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

// Define other methods and classes here
/*

^  1,0,4,0
|  1,3,4,0
|  1,2,3,0
|  1,3,3,0
1  0 -->

*/

void DistributeRandomly(byte[,] cups)
{
	// Create a random ball sequence.
	var ballSequence = CreateBallSequence(cupSize, colorCount);
	Shuffle(ballSequence);
	var ballStack = new Stack<byte>(ballSequence);
	
	// Always take one ball and put it at a random cup.
	var rndCup = new Random();
	while(ballStack.Any())
	{
		var ball = ballStack.Pop();
		var cup = rndCup.Next(cupCount);
		while(!Put(cups, ball, cup))
		{
			cup = rndCup.Next(cupCount);
		}
	}
}

private Boolean Put(byte[,] cups, byte cupColor, int column)
{
	for(byte i=0; i<cupSize; i++)
	{
		if(cups[column, i] == 0)
		{
			cups[column, i] = cupColor;
			return true;
		}
	}
	return false;
}

private void Shuffle(byte[] array)
{
	var rng = new Random();
	rng.Shuffle(array);
}

private byte[] CreateBallSequence(byte cupSize, byte colorCount)
{
	var allBalls = new byte[colorCount * cupSize];
	var i = 0;
	for (byte c = 0; c < colorCount; c++)
	{
		for (byte s = 0; s < cupSize; s++)
		{
			allBalls[i] = (byte)(c + 1);
			i++;
		}
	}
	return allBalls;
}

static class RandomExtensions
{
	public static void Shuffle<T>(this Random rng, T[] array)
	{
		int n = array.Length;
		while (n > 1)
		{
			int k = rng.Next(n--);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}
}
}

class SolvingStep
{
	public byte[,] Board { get; set; }
	public Move LastMove { get; set; }
}

class Move
{
	public byte From { get; set; }
	public byte To { get; set; }
}

class CreationStatistics
{
    public List<List<SolvingStep>> Solutions { get; set; }
    public int nodeCount { get; set; }
}