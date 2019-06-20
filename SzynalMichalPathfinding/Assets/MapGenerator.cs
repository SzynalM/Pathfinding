using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private Transform mapOrigin;
    [SerializeField]
    private Transform nodeParent;
    [SerializeField]
    private int edgeLength;
    Vector2 startPoint;
    Vector2 endPoint;
    [SerializeField]
    private Sprite startPointSprite;
    [SerializeField]
    private Sprite endPointSprite;

    private IFactory<GameObject, Transform, Vector2, INode> nodeFactory;
    private INode[,] nodes;

    [Inject]
    public void Initialize(IFactory<GameObject, Transform, Vector2, INode> _nodeFactory)
    {
        nodeFactory = _nodeFactory;
    }

    public void GenerateMap()
    {
        nodes = new INode[edgeLength, edgeLength];
        DestroyExistingNodes();
        for (int x = 0; x < edgeLength; x++)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                nodes[x, y] = nodeFactory.Create(nodePrefab, nodeParent, new Vector2(mapOrigin.position.x + x, mapOrigin.position.y + y));
            }
        }

        SetNeighbours();
        SetObstacles(4);
        SetStart();
        SetEnd();
    }

    private void DestroyExistingNodes()
    {
        if (nodeParent.childCount > 0)
        {
            for (int i = 0; i < nodeParent.childCount; i++)
            {
                Destroy(nodeParent.GetChild(i).gameObject);
            }
        }
    }

    private void SetStart()
    {
        int xPos = UnityEngine.Random.Range(0, edgeLength - 1);
        int yPos = UnityEngine.Random.Range(0, edgeLength - 1);

        while (nodes[xPos, yPos].IsObstructed)
        {
            xPos = UnityEngine.Random.Range(0, edgeLength - 1);
            yPos = UnityEngine.Random.Range(0, edgeLength - 1);
        }
        if (nodes[xPos, yPos].IsObstructed == false)
        {
            startPoint = new Vector2(xPos, yPos);
            nodes[xPos, yPos].spriteRenderer.sprite = startPointSprite;
        }
    }
    private void SetEnd()
    {
        int xPos = UnityEngine.Random.Range(0, edgeLength - 1);
        int yPos = UnityEngine.Random.Range(0, edgeLength - 1);
        while (nodes[xPos, yPos].IsObstructed)
        {
            xPos = UnityEngine.Random.Range(0, edgeLength - 1);
            yPos = UnityEngine.Random.Range(0, edgeLength - 1);
            if((xPos == startPoint.x && yPos == startPoint.y))
            {
                continue;
            }
        }
        if (nodes[xPos, yPos].IsObstructed == false)
        {
            endPoint = new Vector2(xPos, yPos);
            nodes[xPos, yPos].spriteRenderer.sprite = endPointSprite;
        }
    }

    private void SetObstacles(int obstacleAmount)
    {
        for (int i = 0; i < obstacleAmount; i++)
        {
            int xObstaclePosition = UnityEngine.Random.Range(0, edgeLength - 1);
            int yObstaclePosition = UnityEngine.Random.Range(0, edgeLength - 1);
            int obstacleWidth = UnityEngine.Random.value > 0.3f ? 1 : 0;
            int obstacleHeight = UnityEngine.Random.value > 0.3f ? 1 : 0;

            if (nodes[xObstaclePosition, yObstaclePosition].IsObstructed == false)
            {
                Color color = UnityEngine.Random.ColorHSV();
                Obstacle obstacle = new Obstacle(new Vector2Int(xObstaclePosition, yObstaclePosition), obstacleWidth, obstacleHeight);
                foreach (Vector2Int position in obstacle.GetObstacleVolume())
                {
                    nodes[position.x, position.y].color = color;
                    nodes[position.x, position.y].IsObstructed = true;
                }
            }
            else
            {
                i--;
            }
        }
    }

    private void SetNeighbours()
    {
        for (int x = 0; x < edgeLength; x++)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                AssignNeightboursToNode(x, y);
            }
        }
    }

    private void AssignNeightboursToNode(int x, int y)
    {
        nodes[x, y].Neighbours = new List<Vector2>();
        if (x - 1 >= 0)
        {
            nodes[x, y].Neighbours.Add(new Vector2(x - 1, y));
        }
        if (x + 1 < edgeLength)
        {
            nodes[x, y].Neighbours.Add(new Vector2(x + 1, y));
        }
        if (y - 1 >= 0)
        {
            nodes[x, y].Neighbours.Add(new Vector2(x, y - 1));
        }
        if (y + 1 < edgeLength)
        {
            nodes[x, y].Neighbours.Add(new Vector2(x, y + 1));
        }
    }
}
