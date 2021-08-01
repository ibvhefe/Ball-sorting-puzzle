using System.Collections.Generic;
public class BoardComparer : IEqualityComparer<byte[,]>
{
    byte cupCount;
    byte cupSize;

    public BoardComparer(byte cupCount, byte cupSize)
    {
        this.cupCount = cupCount;
        this.cupSize = cupSize;
    }

	public bool Equals(byte[,] a, byte[,] b)
	{
		for (byte i = 0; i <= this.cupCount-1; i++)
		{
			for (byte j = 0; j <= this.cupSize-1; j++)
			{
				if (a[i, j] != b[i, j])
				{
					return false;
				}
			}
		}

		return true;
	}

	public int GetHashCode(byte[,] obj)
	{
		return obj[0,0];
	}
}