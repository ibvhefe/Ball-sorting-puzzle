class ConsolePrinter
{
  void Print(List<SolvingStep> solution)
  {
    for(var j=0; j<=solution.First().GetLength(1)-1; j++)
    {
    for(var s=0; s<=solution.length-1; s++)
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
    }
    Console.WriteLine();
    }
  }
}