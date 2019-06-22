using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            foreach (DijkstraNode neighbour in neighbours)
            {
                DijkstraNode node = neighbour.GetComponent<DijkstraNode>();

                if (unexploredNodes.Contains(neighbour) && !node.IsObstructed)
                {
                    int distance = NodeDistanceCalculator.GetDistance(neighbour.Position, currentNode.Position);
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
        Vector2 previousPosition = new Vector2();
        while (currentNode != a_StartingNode)
        {
            if(currentNode == null)
            {
                Debug.LogError("No path found");
                return;
            }
            currentNode = currentNode.Parent as DijkstraNode;
            FinalPath.Add(currentNode);
        }
        FinalPath.Reverse();
        FinalPath[0].SpriteRenderer.color = Color.green;
        for (int i = 1; i < FinalPath.Count; i++)
        {
            DijkstraNode node = FinalPath[i];
            previousPosition = FinalPath[i -1].Position;
            node.SpriteRenderer.color = Color.green;
            LineRenderer currentRenderer = node.lines.Where(x => (previousPosition - node.Position) * 10 == (Vector2)x.GetPosition(1)).First();
            currentRenderer.startColor = Color.black;
            currentRenderer.endColor = Color.black;
            currentRenderer.startWidth = .2f;
            currentRenderer.endWidth = .2f;
            currentRenderer.sortingOrder = -1;
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