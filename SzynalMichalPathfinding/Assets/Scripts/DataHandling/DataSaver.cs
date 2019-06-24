using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Pathfinding;
using UI;

namespace DataHandling
{
    public class DataSaver
    {
        private string path;
        private string fileName = "SavedMap.json";

        public void SaveGame(SaveMapClickedSignal saveMapClickedInfo)
        {
            NodeSaveInfo[,] nodesToSave = GetNodes(saveMapClickedInfo.nodesToSave, saveMapClickedInfo.nodesToSave.GetLength(0));
            path = Path.Combine(Application.dataPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string json = JsonConvert.SerializeObject(nodesToSave, Formatting.Indented);
            StreamWriter sr = new StreamWriter(path);
            sr.Write(json);
            sr.Close();
        }

        private NodeSaveInfo[,] GetNodes(Node[,] nodes, int length)
        {
            NodeSaveInfo[,] result = new NodeSaveInfo[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    NodeSaveInfo node = new NodeSaveInfo()
                    {
                        Position = nodes[i, j].Position,
                        IsObstructed = nodes[i, j].IsObstructed,
                        IsStartPoint = nodes[i, j].IsStartPoint,
                        IsEndPoint = nodes[i, j].IsEndPoint,
                    };
                    result[i, j] = node;
                }
            }
            return result;
        }
    }

}