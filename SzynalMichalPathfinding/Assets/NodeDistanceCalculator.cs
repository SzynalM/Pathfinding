using UnityEngine;

public class NodeDistanceCalculator
{
    public int GetDistance(Vector2 from, Vector2 to)
    {
        return (int)((from.x - to.x) + (from.y - to.y));
    }
}