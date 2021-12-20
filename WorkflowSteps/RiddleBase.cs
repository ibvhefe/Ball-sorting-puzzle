using System;
public abstract class RiddleBase
{
    protected int cupSize;
	protected int cupCount;
	protected int colorCount;

	protected static int arraySize;
	protected static int solvingStepSize;

	public RiddleBase(int cupSize, int cupCount, int colorCount)
	{
		this.cupSize = cupSize;
		this.cupCount = cupCount;
		this.colorCount = colorCount;
		arraySize = cupSize*cupCount; 
		solvingStepSize = arraySize + 2;
	}

	protected Boolean Push(byte[,] cups, byte cupColor, int column)
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
}