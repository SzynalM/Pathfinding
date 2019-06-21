using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathfinding : MonoBehaviour, IPathfinder
{
    private List<DijkstraNode> unexploredNodes = new List<DijkstraNode>();
    [SerializeField]
    private MapGenerator mapGenerator;
    private Vector2 startPointIndexes;
    private const int pathWeight = 1; //as per requirements

    public void FindPath(Vector2 startPoint, Vector2 endPoint)
    {
        startPointIndexes = startPoint;
        GetDijkstraNodes();
        DijkstraNode startNode = mapGenerator.nodes[(int)startPoint.x, (int)startPoint.y] as DijkstraNode;
        DijkstraNode endNode = mapGenerator.nodes[(int)endPoint.x, (int)endPoint.y] as DijkstraNode;
        List<DijkstraNode> foundNeighbours = new List<DijkstraNode>();

        while (unexploredNodes.Count > 0)
        {
            unexploredNodes.Sort((x, y) => x.DistanceFromStart.CompareTo(y.DistanceFromStart));

            DijkstraNode currentNode = unexploredNodes[0];
            if (currentNode == endNode)
            {
                Debug.Log("Finished");
                GetFinalPath(startNode, endNode);
            }
            unexploredNodes.Remove(currentNode);

            List<DijkstraNode> neighbours = GetNeighbours(currentNode.Neighbours);
            foreach (DijkstraNode neighNode in neighbours)
            {
                DijkstraNode node = neighNode.GetComponent<DijkstraNode>();

                if (unexploredNodes.Contains(neighNode) && !node.IsObstructed)
                {
                    int distance = NodeDistanceCalculator.GetDistance(neighNode.Position, currentNode.Position);
                    distance = currentNode.DistanceFromStart + distance;
                    if (distance < node.DistanceFromStart)
                    {
                        node.DistanceFromStart = distance;
                        node.Parent = currentNode;
                    }
                }
            }
        }
    }

    private List<DijkstraNode> GetNeighbours(List<INode> neighbours)
    {
        List<DijkstraNode> nodes = new List<DijkstraNode>();
        for (int i = 0; i < neighbours.Count; i++)
        {
            nodes.Add(neighbours[i] as DijkstraNode);
        }
        return nodes;
    }

    private void GetFinalPath(DijkstraNode a_StartingNode, DijkstraNode a_EndNode)
    {
        List<DijkstraNode> FinalPath = new List<DijkstraNode>();
        FinalPath.Add(a_EndNode);
        DijkstraNode currentNode = a_EndNode;
        while (currentNode != a_StartingNode)
        {
            currentNode = currentNode.Parent;
            FinalPath.Add(currentNode);
        }
        FinalPath.Reverse();
        foreach (DijkstraNode node in FinalPath)
        {
            node.SpriteRenderer.color = Color.red;
        }
    }

    private void GetDijkstraNodes()
    {
        for (int i = 0; i < mapGenerator.edgeLength; i++)
        {
            for (int j = 0; j < mapGenerator.edgeLength; j++)
            {
                if (mapGenerator.nodes[i, j].IsObstructed == false)
                {
                    if (i == startPointIndexes.x && j == startPointIndexes.y)
                    {
                        (mapGenerator.nodes[i, j] as DijkstraNode).DistanceFromStart = 0;
                    }
                    else
                    {
                        (mapGenerator.nodes[i, j] as DijkstraNode).DistanceFromStart = int.MaxValue;
                    }
                    unexploredNodes.Add(mapGenerator.nodes[i, j] as DijkstraNode);
                }
            }
        }
    }
}