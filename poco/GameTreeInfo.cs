using System.Collections.Generic;

public class GameTreeInfo
{
    public byte ColorCount { get; set; }
    public byte[,] Riddle { get; set; }

    /// All possible solutions.
    public List<List<SolvingStep>> Solutions { get; set; }

    /// Once inside a deadend group, there is no way to win again.
    public List<HashSet<SolvingStep>> DeadendNodeGroups { get; set; }
    
    /// Overall node count.
    public int NodeCount { get; set; }

    /// All nodes that do not belong to deadend groups and not directly to solutions.
    public int CanReachSolutionCount { get; set; }

    /// Total count of all nodes and leafs belonging to solutions.
    public int SolutionNodeCount { get; set; }

    /// Total count of all deadend nodes and leafs.
    public int DeadendNodeCount { get; set; }
}