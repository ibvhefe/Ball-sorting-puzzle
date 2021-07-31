using System.Collections.Generic;
using System;
using System.Linq;

public class RiddleCreator : RiddleBase
{
	public RiddleCreator(byte cupSize, byte cupCount, byte colorCount) : base(cupSize,cupCount,colorCount)
	{
	}

	public byte[,] Create()
	{
		var cups = new byte[cupCount, cupSize];
		DistributeRandomly(cups);
		return cups;
	}

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
			while(!Push(cups, ball, cup))
			{
				cup = rndCup.Next(cupCount);
			}
		}
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
}