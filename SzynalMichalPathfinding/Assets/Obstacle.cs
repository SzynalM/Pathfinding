using System.Collections.Generic;
using UnityEngine;

public class Obstacle
{
    protected Vector2Int position;
    protected int width;
    protected int height;

    public Obstacle(Vector2Int _position, int _width, int _height)
    {
        position = _position;
        width = _width;
        height = _height;
    }

    public List<Vector2Int> GetObstacleVolume()
    {
        List<Vector2Int> indexes = new List<Vector2Int>();
        if (width > 0 && height > 0)
        {
            indexes.Add(position);
            indexes.Add(new Vector2Int(position.x + width, position.y));
            indexes.Add(new Vector2Int(position.x, position.y + height));
            indexes.Add(new Vector2Int(position.x + width, position.y + height));
            return indexes;
        }
        indexes.Add(position);
        if (!indexes.Contains(new Vector2Int(position.x + width, position.y)))
        {
            indexes.Add(new Vector2Int(position.x + width, position.y));
        }
        if (!indexes.Contains(new Vector2Int(position.x, position.y + height)))
        {
            indexes.Add(new Vector2Int(position.x, position.y + height));
        }
        return indexes;
    }
}