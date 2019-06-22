using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DataSaver
{
    private string path;
    private string fileName = "SavedMap.json";

    public void SaveGame(Node[,] nodesToSave)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(path);
        
        string json = JsonConvert.SerializeObject(nodesToSave, Formatting.Indented);
        Debug.Log("Saved content" + json);
        StreamWriter sr = new StreamWriter(path);
        sr.Write(json);
        sr.Close();
    }
}
