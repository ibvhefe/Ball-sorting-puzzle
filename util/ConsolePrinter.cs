using System;
using System.Collections.Generic;
using System.Linq;

public static class ConsolePrinter
{
  public static void Print(GameTreeInfo gameTreeInfo)
  {
    Print(gameTreeInfo.Riddle);
    Console.WriteLine($"Node count: {gameTreeInfo.NodeCount}");
    Console.WriteLine($"Solution node count:{gameTreeInfo.SolutionNodeCount}");
    Console.WriteLine($"Different solutions: {gameTreeInfo.Solutions.Count}");
    foreach(var solution in gameTreeInfo.Solutions)
    {
      Print(solution);
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
          Console.Write(" ");
        }
        Console.Write($"  {content}");
      }
      Console.WriteLine();
    }
  }
  
  public static void Print(List<SolvingStep> solution)
  {
    for(var j=solution.First().Board.GetLength(1)-1; j>=0; j--)
    {
      for(var s=solution.Count-1; s>=0; s--)
      {
        for(var i=0; i<=solution.First().Board.GetLength(0)-1; i++)
        {
          var content = solution[s].Board[i,j];
          if(content>9)
          {
            Console.Write(" ");
          }
          Console.Write($"  {content}");
        }
        Console.Write("   ");
      }
      Console.WriteLine();
    }
    RepeatChar("-",100);
  }
  
  public static void RepeatChar(string character, int count)
  {
    for(var i=0; i<= count-1; i++)
    {
      Console.Write(character);
    }
    Console.WriteLine();
  }
}