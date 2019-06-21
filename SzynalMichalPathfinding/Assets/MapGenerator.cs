using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private DijkstraPathfinding pathfinding;
    [SerializeField]
    private EdgeDisplayer edgeDisplayer;
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private Transform nodeParent;
    public int edgeLength;
    public Vector2 startPoint;
    public Vector2 endPoint;
    [SerializeField]
    private Sprite startPointSprite;
    [SerializeField]
    private Sprite endPointSprite;
    [SerializeField]
    private TMP_InputField edgeLengthInputField;
    [SerializeField]
    private TMP_InputField amountOfObstaclesInputField;

    private IFactory<GameObject, Transform, Vector2, INode> nodeFactory;
    public INode[,] nodes;

    [Inject]
    public void Initialize(IFactory<GameObject, Transform, Vector2, INode> _nodeFactory)
    {
        nodeFactory = _nodeFactory;
    }

    public void GenerateMap()
    {
        if (int.Parse(edgeLengthInputField.text) >= 10)
        {
            edgeLength = int.Parse(edgeLengthInputField.text);
        }
        else
        {
            Debug.LogError("Value too small");
        }
        nodes = new INode[edgeLength, edgeLength];
        DestroyExistingNodes();
        for (int x = 0; x < edgeLength; x++)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                nodes[x, y] = nodeFactory.Create(nodePrefab, nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));
            }
        }

        SetNeighbours();
        SetObstacles(int.Parse(amountOfObstaclesInputField.text));
        SetStart();
        SetEnd();
        edgeDisplayer.ShowLines();
        pathfinding.FindPath(startPoint, endPoint);
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
            nodes[xPos, yPos].SpriteRenderer.sprite = startPointSprite;
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
            if ((xPos == startPoint.x && yPos == startPoint.y))
            {
                continue;
            }
        }
        if (nodes[xPos, yPos].IsObstructed == false)
        {
            endPoint = new Vector2(xPos, yPos);
            nodes[xPos, yPos].SpriteRenderer.sprite = endPointSprite;
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
                    nodes[position.x, position.y].Color = color;
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
        nodes[x, y].Neighbours = new List<INode>();
        if (x - 1 >= 0)
        {
            nodes[x, y].Neighbours.Add(nodes[x - 1, y]);
        }
        if (x + 1 < edgeLength)
        {
            nodes[x, y].Neighbours.Add(nodes[x + 1, y]);
        }
        if (y - 1 >= 0)
        {
            nodes[x, y].Neighbours.Add(nodes[x, y - 1]);
        }
        if (y + 1 < edgeLength)
        {
            nodes[x, y].Neighbours.Add(nodes[x, y + 1]);
        }
    }
}
