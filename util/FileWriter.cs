using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
        return result;
    }
}

public class ExportGameTreeInfo
{
   public System.String worldType;
   public int tubeCount;
   public int tubeSize;
   public int colorCount;
   //public List<List<int>> board;
   public byte[,] board;
}