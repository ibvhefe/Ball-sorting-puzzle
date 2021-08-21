using System.Collections.Generic;
public class BoardComparer : IEqualityComparer<SolvingStep>
{
    byte cupCount;
    byte cupSize;

    public BoardComparer(byte cupCount, byte cupSize)
    {
        this.cupCount = cupCount;
        this.cupSize = cupSize;
    }

	public bool Equals(SolvingStep a, SolvingStep b)
	{
		for (byte i = 0; i <= this.cupCount-1; i++)
		{
			for (byte j = 0; j <= this.cupSize-1; j++)
			{
				if (a.Board[i, j] != b.Board[i, j])
				{
					return false;
				}
			}
		}

		return true;
	}

	public int GetHashCode(SolvingStep obj)
	{
		unchecked // Overflow is fine, just wrap
		{
			int hash = 17;
			for(var i=0;i<this.cupSize;i++)
			{
				hash = hash * 23 + obj.Board[0,i].GetHashCode();
			}
			return hash;
		}
	}
}