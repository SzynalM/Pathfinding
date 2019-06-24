using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(SpriteRenderer)), System.Serializable]
    public class AStarNode : Node
    {
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost { get { return GCost + HCost; } }
        public int DistanceFromEndPoint { get; set; }
    } 
}
