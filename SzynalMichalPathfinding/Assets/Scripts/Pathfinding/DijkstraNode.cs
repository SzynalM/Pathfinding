using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(SpriteRenderer)), System.Serializable]
    public class DijkstraNode : Node
    {
        public int DistanceFromStart { get; set; }
    } 
}