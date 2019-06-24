using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DataSaver
{
    private string path;
    private string fileName = "SavedMap.json";

    public void SaveGame(SaveMapClickedSignal saveMapClickedInfo)
    {
        Node[,] nodesToSave = GetNodes(saveMapClickedInfo.nodesToSave, saveMapClickedInfo.nodesToSave.GetLength(0));
        path = Path.Combine(Application.dataPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        Debug.Log(path);
        
        string json = JsonConvert.SerializeObject(nodesToSave, Formatting.Indented);
        Debug.Log("Saved content" + json);
        StreamWriter sr = new StreamWriter(path);
        sr.Write(json);
        sr.Close();
    }

    private Node[,] GetNodes(INode[,] nodes, int length)
    {
        Node[,] result = new Node[length, length];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Node node = new Node()
                {
                    Position = nodes[i, j].Position,
                    IsObstructed = nodes[i, j].IsObstructed,
                    IsStartPoint = nodes[i, j].IsStartPoint,
                    IsEndPoint = nodes[i, j].IsEndPoint
                };
                result[i, j] = node;
            }
        }
        return result;
    }
}
