using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataLoader : MonoBehaviour
{
    private string path;
    private string fileName = "SavedMap.json";

    public Node[,] LoadMap()
    {
        path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(path);
        Node[,] loadedNodes;
        if (File.Exists(path))
        {
            StreamReader sw = new StreamReader(path);
            string rawLoadedData = sw.ReadToEnd();
            sw.Close();
            loadedNodes = JsonConvert.DeserializeObject<Node[,]>(rawLoadedData);
            return loadedNodes;
        }
        else
        {
            Debug.LogError("No file to load");
            return null;
        }
    }
}