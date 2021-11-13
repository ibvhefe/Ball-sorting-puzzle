using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
public class FileWriter
{
    private readonly string baseFolder;

    public FileWriter(String baseFolder)
    {
        this.baseFolder = baseFolder;
    }
    public void WriteToJson(int levelNumber, GameTreeInfo gameTreeInfo)
    {
        using (StreamWriter file = File.CreateText($"{baseFolder}{levelNumber}.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, Convert(gameTreeInfo, levelNumber));
        }
    }

    private ExportGameTreeInfo Convert(GameTreeInfo gameTreeInfo, int levelNumber)
    {
        var result = new ExportGameTreeInfo();
        result.worldType = getAlternatingWorldType(levelNumber);
        result.tubeCount = gameTreeInfo.Riddle.GetLength(0);
        result.tubeSize = gameTreeInfo.Riddle.GetLength(1);
        result.colorCount = gameTreeInfo.ColorCount;
        result.board = gameTreeInfo.Riddle;
        result.oneStarLimit = gameTreeInfo.OneStarLimit;
        result.twoStarLimit = gameTreeInfo.TwoStarLimit;
        result.threeStarLimit = gameTreeInfo.ThreeStarLimit;
        return result;
    }

    private String getAlternatingWorldType(int levelNumber)
    {
        switch(levelNumber%5)
        {
            case 0: return "Forest";
            case 1: return "Candy";
            case 2: return "Graveyard";
            case 3: return "Ice";
            case 4: return "Desert";
        }
        return "Forest";
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