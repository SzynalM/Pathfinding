using UnityEngine;

public static class NodeDistanceCalculator
{
    public static int GetDistance(Vector2 from, Vector2 to)
    {
        return Mathf.Abs((int)((from.x - to.x) + (from.y - to.y)));
    }
}