using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public class DijkstraPathfinding : MonoBehaviour, IPathfinder
{
    private List<DijkstraNode> unexploredNodes = new List<DijkstraNode>();
    private MapGenerator mapGenerator;
    private SignalBus signalBus;
    private Vector2 startPointPosition;
    private const int pathWeight = 1; //as per requirements

    [Inject]
    public void Initialize(SignalBus _signalBus, MapGenerator _mapGenerator)
    {
        signalBus = _signalBus;
        mapGenerator = _mapGenerator;
    }

    public void FindPath(Vector2 startPosition, Vector2 endPosition)
    {
        Debug.Log("dijk");
        startPointPosition = startPosition;
        GetDijkstraNodes();
        DijkstraNode startNode = mapGenerator.nodes[(int)startPosition.x, (int)startPosition.y] as DijkstraNode;
        DijkstraNode endNode = mapGenerator.nodes[(int)endPosition.x, (int)endPosition.y] as DijkstraNode;
        List<DijkstraNode> foundNeighbours = new List<DijkstraNode>();

        while (unexploredNodes.Count > 0)
        {
            unexploredNodes.Sort((x, y) => x.DistanceFromStart.CompareTo(y.DistanceFromStart));

            DijkstraNode currentNode = unexploredNodes[0];
            if (currentNode == endNode)
            {
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
                signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = WarningMessages.noPathFound });
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
                    if (i == startPointPosition.x && j == startPointPosition.y)
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