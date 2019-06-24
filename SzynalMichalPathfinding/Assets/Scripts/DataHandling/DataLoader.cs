using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace DataHandling
{
    public class DataLoader 
    {
        private string path;
        private string fileName = "SavedMap.json";

        public NodeSaveInfo[,] LoadData()
        {
            path = Path.Combine(Application.dataPath, fileName);
            NodeSaveInfo[,] loadedNodes;
            if (File.Exists(path))
            {
                StreamReader sw = new StreamReader(path);
                string rawLoadedData = sw.ReadToEnd();
                sw.Close();
                loadedNodes = JsonConvert.DeserializeObject<NodeSaveInfo[,]>(rawLoadedData);
                return loadedNodes;
            }
            else
            {
                Debug.LogError("No file to load");
                return null;
            }
        }
    } 
}