using UnityEngine;

namespace Pathfinding
{
    public interface IPathfinder
    {
        void FindPath(Vector2 startPosition, Vector2 endPosition);
    } 
}
