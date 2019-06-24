using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class Obstacle
    {
        private Vector2Int position;
        private int width;
        private int height;

        public Obstacle(Vector2Int _position, int _width, int _height)
        {
            position = _position;
            width = _width;
            height = _height;
        }

        public List<Vector2Int> GetObstacleVolume()
        {
            List<Vector2Int> indices = new List<Vector2Int>();
            if (width > 0 && height > 0)
            {
                indices.Add(position);
                indices.Add(new Vector2Int(position.x + width, position.y));
                indices.Add(new Vector2Int(position.x, position.y + height));
                indices.Add(new Vector2Int(position.x + width, position.y + height));
                return indices;
            }
            indices.Add(position);

            if (!indices.Contains(new Vector2Int(position.x + width, position.y)))
            {
                indices.Add(new Vector2Int(position.x + width, position.y));
            }
            if (!indices.Contains(new Vector2Int(position.x, position.y + height)))
            {
                indices.Add(new Vector2Int(position.x, position.y + height));
            }
            return indices;
        }
    } 
}