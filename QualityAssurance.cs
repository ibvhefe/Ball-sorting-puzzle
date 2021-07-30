using System.Linq;
using System;
public static class QualityAssurance
{
	public static void Check(GameTreeInfo statistics)
	{
		var count = statistics.Solutions.Count;
		var distinctCount = statistics.Solutions.Distinct(new SolutionComparer()).Count();
		Console.Write(count == distinctCount);
	}
}