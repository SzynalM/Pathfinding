using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

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
        AStarNode StartNode = mapGenerator.nodes[(int)startPosition.x, (int)startPosition.y] as AStarNode;
        AStarNode TargetNode = mapGenerator.nodes[(int)endPosition.x, (int)endPosition.y] as AStarNode;

        List<AStarNode> OpenList = new List<AStarNode>();
        HashSet<AStarNode> ClosedList = new HashSet<AStarNode>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            AStarNode CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F_Cost < CurrentNode.F_Cost || OpenList[i].F_Cost == CurrentNode.F_Cost && OpenList[i].H_Cost < CurrentNode.H_Cost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
                return;
            }

            foreach (AStarNode NeighborNode in CurrentNode.Neighbours)
            {
                if (NeighborNode.IsObstructed || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = (int)CurrentNode.G_Cost + NodeDistanceCalculator.GetDistance(CurrentNode.Position, NeighborNode.Position);

                if (MoveCost < NeighborNode.G_Cost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.G_Cost = MoveCost;
                    NeighborNode.H_Cost = NodeDistanceCalculator.GetDistance(NeighborNode.Position, TargetNode.Position);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }



    void GetFinalPath(AStarNode a_StartingNode, AStarNode a_EndNode)
    {
        List<AStarNode> FinalPath = new List<AStarNode>();
        FinalPath.Add(a_EndNode);
        AStarNode currentNode = a_EndNode;
        Vector2 previousPosition = new Vector2();
        while (currentNode != a_StartingNode)
        {
            if (currentNode == null)
            {
                signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = WarningMessages.noPathFound });
                return;
            }
            currentNode = currentNode.Parent as AStarNode;
            FinalPath.Add(currentNode);
        }
        FinalPath.Reverse();
        FinalPath[0].SpriteRenderer.color = Color.green;
        for (int i = 1; i < FinalPath.Count; i++)
        {
            AStarNode node = FinalPath[i];
            previousPosition = FinalPath[i - 1].Position;
            node.SpriteRenderer.color = Color.green;
            LineRenderer currentRenderer = node.lines.Where(x => (previousPosition - node.Position) * 10 == (Vector2)x.GetPosition(1)).First();
            currentRenderer.startColor = Color.black;
            currentRenderer.endColor = Color.black;
            currentRenderer.startWidth = .2f;
            currentRenderer.endWidth = .2f;
            currentRenderer.sortingOrder = -1;
        }
    }
}
