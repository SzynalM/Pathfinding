using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataLoader : IDataLoader
{
    private string path;
    private string fileName = "SavedMap.json";

    public Node[,] LoadData()
    {
        path = Path.Combine(Application.dataPath, fileName);
        Node[,] loadedNodes;
        if (File.Exists(path))
        {
            StreamReader sw = new StreamReader(path);
            string rawLoadedData = sw.ReadToEnd();
            sw.Close();
            loadedNodes = JsonConvert.DeserializeObject<Node[,]>(rawLoadedData);
            int x = 0;
            foreach(Node node in loadedNodes)
            {
                x++;
            }

            Debug.Log("Loaded nodes amount: " + x);
            return loadedNodes;
        }
        else
        {
            Debug.LogError("No file to load");
            return null;
        }
    }
}

[CreateAssetMenu(fileName = "GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    public string saveFileName;
    public Sprite startNodeSprite;
    public Sprite endNodeSprite;
    public Color foundPathColor;
    public Color obstructedNodesColor;
}