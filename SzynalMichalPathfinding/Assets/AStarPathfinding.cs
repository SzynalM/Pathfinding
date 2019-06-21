using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField]
    MapGenerator mapGenerator;

    public void FindPath(Vector3 startPoint, Vector3 endPoint)
    {
        AStarNode StartNode = (AStarNode)mapGenerator.nodes[(int)startPoint.x, (int)startPoint.y];
        AStarNode TargetNode = (AStarNode)mapGenerator.nodes[(int)endPoint.x, (int)endPoint.y];

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
        AStarNode CurrentNode = a_EndNode;

        while (CurrentNode != a_StartingNode)
        {
            CurrentNode = CurrentNode.Parent;
            FinalPath.Add(CurrentNode);
        }

        FinalPath.Reverse();
        foreach (AStarNode node in FinalPath)
        {
            node.SpriteRenderer.color = Color.red;
        }
    }
}
