using System.Collections.Generic;

public class GameTreeInfo
{
    public byte[,] Riddle { get; set; }
    public List<List<SolvingStep>> Solutions { get; set; }
    public List<HashSet<SolvingStep>> DeadendNodeGroups { get; set; }
    public int NodeCount { get; set; }
    public int CanReachSolutionCount { get; set; }
    public int SolutionNodeCount { get; set; }
    public int DeadendNodeCount { get; set; }
}