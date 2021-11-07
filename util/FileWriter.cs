using Newtonsoft.Json;
using System.IO;
public static class FileWriter
{
    public static void WriteToJson(string filePath, GameTreeInfo gameTreeInfo)
    {
        using (StreamWriter file = File.CreateText(filePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, gameTreeInfo);
        }
    }
}