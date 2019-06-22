using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown algorithChoice;
    [SerializeField]
    private DijkstraPathfinding dijkstraPathfinding;
    [SerializeField]
    private AStarPathfinding aStarPathfinding;
    [SerializeField]
    private GameObject dijkstraNodePrefab;
    [SerializeField]
    private GameObject astarNodePrefab;
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
    [SerializeField]
    private DataLoader dataLoader;

    private IFactory<GameObject, Transform, Vector2, INode> nodeFactory;
    public INode[,] nodes;

    [Inject]
    public void Initialize(IFactory<GameObject, Transform, Vector2, INode> _nodeFactory)
    {
        nodeFactory = _nodeFactory;
    }

    public void GenerateMap(bool Loaded)
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
        if (Loaded)
        {
            Node[,] loadedNodes = dataLoader.LoadMap();


            for (int x = 0; x < edgeLength; x++)
            {
                for (int y = 0; y < edgeLength; y++)
                {
                    Debug.LogWarning(loadedNodes[x, y].Position + " " + loadedNodes[x, y].IsEndPoint + " " + loadedNodes[x, y].IsStartPoint + " " + loadedNodes[x, y].IsObstructed);
                    if (algorithChoice.value == 0)
                    {
                        nodes[x, y] = nodeFactory.Create(dijkstraNodePrefab, nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));

                    }
                    else
                    {
                        nodes[x, y] = nodeFactory.Create(astarNodePrefab, nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));

                    }
                    nodes[x, y].IsObstructed = loadedNodes[x, y].IsObstructed;
                    nodes[x, y].Parent = loadedNodes[x, y].Parent;
                    nodes[x, y].IsStartPoint = loadedNodes[x, y].IsStartPoint;
                    nodes[x, y].IsEndPoint = loadedNodes[x, y].IsEndPoint;
                    if (nodes[x, y].IsStartPoint == true)
                    {
                        startPoint = nodes[x, y].Position;
                    }
                    if (nodes[x, y].IsEndPoint == true)
                    {
                        endPoint = nodes[x, y].Position;
                    }
                }
            }
            SetNeighbours();
            if (algorithChoice.value == 0)
            {
                dijkstraPathfinding.FindPath(startPoint, endPoint);
            }
            else
            {
                aStarPathfinding.FindPath(startPoint, endPoint);
            }
            return;
        }
        for (int x = 0; x < edgeLength; x++)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                if (algorithChoice.value == 0)
                {
                    nodes[x, y] = nodeFactory.Create(dijkstraNodePrefab, nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));

                }
                else
                {
                    nodes[x, y] = nodeFactory.Create(astarNodePrefab, nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));

                }
            }
        }
        SetNeighbours();
        SetObstacles(int.Parse(amountOfObstaclesInputField.text));
        SetStart();
        SetEnd();
        Debug.LogWarning("Algo choice value" + algorithChoice.value);
        if (algorithChoice.value == 0)
        {
            Debug.LogWarning("Dijkstra chosen");
            dijkstraPathfinding.FindPath(startPoint, endPoint);
        }
        else
        {
            Debug.LogWarning("Astar chosen");
            aStarPathfinding.FindPath(startPoint, endPoint);
        }
    }

    public void LoadData()
    {
        dataLoader.LoadMap();
    }

    public void SaveNodesToFile()
    {
        DataSaver ds = new DataSaver();
        ds.SaveGame(GetNodes(nodes, edgeLength));
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
            nodes[xPos, yPos].IsStartPoint = true;
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
            nodes[xPos, yPos].IsEndPoint = true;
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
                    nodes[position.x, position.y].IsObstructed = true;
                }
            }
            else
            {
                i--;
            }
        }
    }

    private Node[,] GetNodes(INode[,] nodes, int length)
    {
        Node[,] result = new Node[length, length];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Node node = new Node()
                {
                    Position = nodes[i, j].Position,
                    IsObstructed = nodes[i, j].IsObstructed,
                    IsStartPoint = nodes[i, j].IsStartPoint,
                    IsEndPoint = nodes[i, j].IsEndPoint
                };
                result[i, j] = node;
                Debug.Log("result: " + result[i, j].Position);
            }
        }
        return result;
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
        List<INode> currentNeighbours = new List<INode>();
        if (x - 1 >= 0)
        {
            currentNeighbours.Add(nodes[x - 1, y]);
        }
        if (x + 1 < edgeLength)
        {
            currentNeighbours.Add(nodes[x + 1, y]);
        }
        if (y - 1 >= 0)
        {
            currentNeighbours.Add(nodes[x, y - 1]);
        }
        if (y + 1 < edgeLength)
        {
            currentNeighbours.Add(nodes[x, y + 1]);
        }
        nodes[x, y].Neighbours = currentNeighbours;
    }
}

[Serializable]
public class Node
{
    public Vector2 Position { get; set; }
    public bool IsObstructed { get; set; }
    public INode Parent { get; set; }
    public bool IsStartPoint { get; set; }
    public bool IsEndPoint { get; set; }
}
