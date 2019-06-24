using UnityEngine;

namespace DataHandling
{
    [System.Serializable]
    public class NodeSaveInfo
    {
        public Vector2 Position { get; set; }
        public bool IsObstructed { get; set; }
        public bool IsStartPoint { get; set; }
        public bool IsEndPoint { get; set; }
    } 
}