using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator
{
    private int obstaclesSet;
    private int maxObstacles;
    private int edgeLength;
    private INode[,] nodes;

    public ObstacleGenerator(INode[,] _nodes, int _maxObstacles, int _edgeLength)
    {
        maxObstacles = _maxObstacles;
        edgeLength = _edgeLength;
        nodes = _nodes;
   
    }
}