using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MapGeneration;

namespace Pathfinding
{
    public class AStarPathfinding : MonoBehaviour, IPathfinder
    {
        private SignalBus signalBus;
        private MapGenerator mapGenerator;

        [Inject]
        public void Initialize(SignalBus _signalBus, MapGenerator _mapGenerator)
        {
            signalBus = _signalBus;
            mapGenerator = _mapGenerator;
        }

        public void FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            AStarNode startNode = mapGenerator.nodes[(int)startPosition.x, (int)startPosition.y] as AStarNode;
            AStarNode endNode = mapGenerator.nodes[(int)endPosition.x, (int)endPosition.y] as AStarNode;

            List<AStarNode> unvisitedNodes = new List<AStarNode>();
            HashSet<AStarNode> visitedNodes = new HashSet<AStarNode>();

            unvisitedNodes.Add(startNode);

            while (unvisitedNodes.Count > 0)
            {
                AStarNode currentNode = unvisitedNodes[0];
                for (int i = 1; i < unvisitedNodes.Count; i++)
                {
                    if (unvisitedNodes[i].FCost < currentNode.FCost || unvisitedNodes[i].FCost == currentNode.FCost && unvisitedNodes[i].HCost < currentNode.HCost)
                    {
                        currentNode = unvisitedNodes[i];
                    }
                }
                unvisitedNodes.Remove(currentNode);
                visitedNodes.Add(currentNode);

                if (currentNode == endNode)
                {
                    signalBus.Fire(new PathFoundSignal() { startingNode = startNode, endNode = endNode });
                    return;
                }

                foreach (AStarNode neighbour in currentNode.Neighbours)
                {
                    if (neighbour.IsObstructed || visitedNodes.Contains(neighbour))
                    {
                        continue;
                    }
                    int MoveCost = (int)currentNode.GCost + NodeDistanceCalculator.GetDistance(currentNode.Position, neighbour.Position);

                    if (MoveCost < neighbour.GCost || !unvisitedNodes.Contains(neighbour))
                    {
                        neighbour.GCost = MoveCost;
                        neighbour.HCost = NodeDistanceCalculator.GetDistance(neighbour.Position, endNode.Position);
                        neighbour.Parent = currentNode;

                        if (!unvisitedNodes.Contains(neighbour))
                        {
                            unvisitedNodes.Add(neighbour);
                        }
                    }
                }
            }
        }
    } 
}