using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public static class FileWriter
{
    public static void WriteToJson(string filePath, GameTreeInfo gameTreeInfo)
    {
        using (StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, Convert(gameTreeInfo));
        }
    }

    private static ExportGameTreeInfo Convert(GameTreeInfo gameTreeInfo)
    {
        var result = new ExportGameTreeInfo();
        result.worldType = "Forest";
        result.tubeCount = gameTreeInfo.Riddle.GetLength(0);
        result.tubeSize = gameTreeInfo.Riddle.GetLength(1);
        result.colorCount = gameTreeInfo.ColorCount;
        result.board = gameTreeInfo.Riddle;
        result.oneStarLimit = gameTreeInfo.OneStarLimit;
        result.twoStarLimit = gameTreeInfo.TwoStarLimit;
        result.threeStarLimit = gameTreeInfo.ThreeStarLimit;
        return result;
    }
}

public class ExportGameTreeInfo
{
   public System.String worldType;
   public int tubeCount;
   public int tubeSize;
   public int colorCount;
   public byte[,] board;

   public int threeStarLimit {get;set;}
   public int twoStarLimit { get; set; }
   public int oneStarLimit { get; set; }
}