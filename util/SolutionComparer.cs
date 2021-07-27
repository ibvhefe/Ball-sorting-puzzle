using System.Collections.Generic;
public class SolutionComparer : IEqualityComparer<List<SolvingStep>>
{
	public bool Equals(List<SolvingStep> a, List<SolvingStep> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}

		for (var i = 0; i <= a.Count - 1; i++)
		{
			if (a[i].LastMove.From != b[i].LastMove.From)
			{
				return false;
			}

			if (a[i].LastMove.To != b[i].LastMove.To)
			{
				return false;
			}
		}

		return true;
	}

	public int GetHashCode(List<SolvingStep> obj)
	{
		//var hash = obj.GetHashCode();
		return obj.Count;
	}
}