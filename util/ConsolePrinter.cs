using System;
using System.Collections.Generic;
using System.Linq;

public static class ConsolePrinter
{
  private static Boolean debugMode = false;
  public static void SetDebugMode()
  {
    debugMode = true;
  }

  private static void Write(String content)
  {
    if(debugMode)
    {
      System.Diagnostics.Debug.Write(content);
    }
    else
    {
      Console.Write(content);
    }
  }

  private static void WriteLine(String content)
  {
    if(debugMode)
    {
      System.Diagnostics.Debug.WriteLine(content);
    }
    else
    {
      Console.WriteLine(content);
    }
  }

  public static void Print(GameTreeInfo gameTreeInfo)
  {
    Print(gameTreeInfo.Riddle);
    WriteLine("");
    WriteLine($"Solution node count: {gameTreeInfo.SolutionNodeCount}");
    WriteLine($"Dead end node count: {gameTreeInfo.DeadendNodeCount}");
    WriteLine($"Can reach solution node count: {gameTreeInfo.CanReachSolutionCount}");
    WriteLine($"Total node count: {gameTreeInfo.NodeCount}");
    WriteLine("");
    WriteLine($"Different solutions: {gameTreeInfo.Solutions.Count}");
    foreach(var solution in gameTreeInfo.Solutions)
    {
      Print(solution.Select(s => s.Board).ToList());
    }
    WriteLine($"Deadends: {gameTreeInfo.DeadendNodeCount}");
    if(gameTreeInfo.DeadendNodeCount>0)
    {
      foreach(var group in gameTreeInfo.DeadendNodeGroups)
      {
        Print(group.Select(d=>d.Board).ToList());
      }
    }
    WriteLine($"Can reach solution nodes: {gameTreeInfo.CanReachSolutionCount}");
    if(gameTreeInfo.CanReachSolutionCount>0)
    {
      foreach(var canReachSolutionNode in gameTreeInfo.CanReachSolutions)
      {
        Print(canReachSolutionNode.Board);
        WriteLine("");
      }
    }
  }
  
  public static void Print(byte[,] array)
  {
    for(var i=array.GetLength(1)-1; i>= 0; i--)
    {
      for(var j=0; j<=array.GetLength(0)-1; j++)
      {
        var content =array[j,i];
        if(content>9)
        {
          Write(" ");
        }
        Write($"  {content}");
      }
      WriteLine("");
    }
  }
  
  public static void Print(IList<byte[,]> boards)
  {
    for(var j=boards.First().GetLength(1)-1; j>=0; j--)
    {
      for(var s=boards.Count-1; s>=0; s--)
      {
        for(var i=0; i<=boards.First().GetLength(0)-1; i++)
        {
          var content = boards[s][i,j];
          if(content>9)
          {
            Write(" ");
          }
          Write($"  {content}");
        }
        Write("   ");
      }
      WriteLine("");
    }
    RepeatChar("-",100);
  }
  
  public static void RepeatChar(string character, int count)
  {
    for(var i=0; i<= count-1; i++)
    {
      Write(character);
    }
    WriteLine("");
  }
}