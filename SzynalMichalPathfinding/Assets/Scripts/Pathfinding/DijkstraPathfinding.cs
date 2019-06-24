using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MapGeneration;

namespace Pathfinding
{
    public class DijkstraPathfinding : MonoBehaviour
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
            startPointPosition = startPosition;
            GetDijkstraNodes();
            DijkstraNode startNode = mapGenerator.nodes[(int)startPosition.x, (int)startPosition.y] as DijkstraNode;
            DijkstraNode endNode = mapGenerator.nodes[(int)endPosition.x, (int)endPosition.y] as DijkstraNode;

            while (unexploredNodes.Count > 0)
            {
                unexploredNodes.Sort((x, y) => x.DistanceFromStart.CompareTo(y.DistanceFromStart));

                DijkstraNode currentNode = unexploredNodes[0];
                if (currentNode == endNode)
                {
                    signalBus.Fire(new PathFoundSignal() { startingNode = startNode, endNode = endNode });
                    return;
                }
                unexploredNodes.Remove(currentNode);

                List<DijkstraNode> neighbours = GetNeighbours(currentNode.Neighbours);
                foreach (DijkstraNode neighbour in neighbours)
                {
                    if (neighbour == null)
                    {
                        continue;
                    }
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

        private List<DijkstraNode> GetNeighbours(List<Node> neighbours)
        {
            List<DijkstraNode> nodes = new List<DijkstraNode>();
            for (int i = 0; i < neighbours.Count; i++)
            {
                nodes.Add(neighbours[i] as DijkstraNode);
            }
            return nodes;
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
}