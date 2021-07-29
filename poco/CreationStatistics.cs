using System.Collections.Generic;
public class CreationStatistics
{
    public byte[,] Riddle { get; set; }
    public List<List<SolvingStep>> Solutions { get; set; }
    public int NodeCount { get; set; }
    public int SolutionNodeCount { get; set; }
}