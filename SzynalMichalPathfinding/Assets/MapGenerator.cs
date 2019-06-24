using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MapGenerator : MonoBehaviour
{
    private IFactory<GameObject, Transform, Vector2, INode> nodeFactory;
    private MapGeneratedSignal onMapGenerated;
    private GameObject nodePrefab;
    private SignalBus signalBus;
    public int edgeLength;
    private int amountOfObstacles;
    private int algorithmIndex; 

    [SerializeField]
    private Transform nodeParent;
    private DijkstraNode dijkstraNodePrefab;
    private AStarNode astarNodePrefab;

    public INode[,] nodes;

    private Vector2 startPoint;
    private Vector2 endPoint;

    [Inject]
    public void Initialize(IFactory<GameObject, Transform, Vector2, INode> _nodeFactory, DijkstraNode _dijkstraNodePrefab, AStarNode _astarNodePrefab, SignalBus _signalBus)
    {
        nodeFactory = _nodeFactory;
        signalBus = _signalBus;
        dijkstraNodePrefab = _dijkstraNodePrefab;
        astarNodePrefab = _astarNodePrefab;
    }

    public void GenerateMap(LoadMapClickedSignal loadMapClickedInfo)
    {
        algorithmIndex = loadMapClickedInfo.algorithmIndex;
        Debug.Log("generate map load");
        RemoveExistingNodes();
        Node[,] loadedNodes = new DataLoader().LoadData();
        edgeLength = loadedNodes.GetLength(0);
        Debug.Log("Loaded edgelength" + edgeLength);
        nodes = new INode[edgeLength, edgeLength];
        for (int x = 0; x < edgeLength; x++)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                SetupNode(loadedNodes, x, y);
            }
        }
        SetNeighbours();
        signalBus.Fire(new MapGeneratedSignal() {  starPosition = startPoint, endPosition = endPoint});
    }


    public void GenerateMap(GenerationDataValidatedSignal generateMapClickedInfo)
    {        
        RemoveExistingNodes();
        Debug.Log("generating map classic| edgelength" + generateMapClickedInfo.edgeLength);
        edgeLength = generateMapClickedInfo.edgeLength;
        nodes = new INode[edgeLength, edgeLength];
        amountOfObstacles = generateMapClickedInfo.amountOfObstacles;
        algorithmIndex = generateMapClickedInfo.algorithmIndex;
        for (int x = 0; x < edgeLength; x++)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                Debug.Log("Prefab: " + GetNodePrefab(algorithmIndex).ToString() + " index: + " + algorithmIndex);
                nodes[x, y] = nodeFactory.Create(GetNodePrefab(algorithmIndex), nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));
            }
        }
        SetNeighbours();
        SetObstacles();
        SetStartNode();
        SetEndNode();
        signalBus.Fire(new MapGeneratedSignal() { starPosition = startPoint, endPosition = endPoint, edgeLength = edgeLength });
    }

    private GameObject GetNodePrefab(int algorithmIndex)
    {
        if(algorithmIndex == 0)
        {
            return dijkstraNodePrefab.gameObject;
        }
        else if (algorithmIndex == 1)
        {
            return astarNodePrefab.gameObject;
        }
        return null;
    }

    private void SetupNode(Node[,] loadedNodes, int x, int y)
    {
        nodes[x, y] = nodeFactory.Create(GetNodePrefab(algorithmIndex), nodeParent, new Vector2(nodeParent.position.x + x, nodeParent.position.y + y));
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

    private void RemoveExistingNodes()
    {
        if (nodeParent.childCount > 0)
        {
            for (int i = 0; i < nodeParent.childCount; i++)
            {
                Destroy(nodeParent.GetChild(i).gameObject);
            }
        }
    }

    private void SetStartNode()
    {
        int xPos = Random.Range(0, edgeLength - 1);
        int yPos = Random.Range(0, edgeLength - 1);

        while (nodes[xPos, yPos].IsObstructed)
        {
            xPos = Random.Range(0, edgeLength - 1);
            yPos = Random.Range(0, edgeLength - 1);
        }
        if (nodes[xPos, yPos].IsObstructed == false)
        {
            startPoint = new Vector2(xPos, yPos);
            nodes[xPos, yPos].IsStartPoint = true;
        }
    }
    private void SetEndNode()
    {
        int xPos = Random.Range(0, edgeLength - 1);
        int yPos = Random.Range(0, edgeLength - 1);
        while (nodes[xPos, yPos].IsObstructed)
        {
            xPos = Random.Range(0, edgeLength - 1);
            yPos = Random.Range(0, edgeLength - 1);
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

    private void SetObstacles()
    {
        for (int i = 0; i < amountOfObstacles; i++)
        {
            int xObstaclePosition = Random.Range(0, edgeLength - 1);
            int yObstaclePosition = Random.Range(0, edgeLength - 1);
            int obstacleWidth = Random.value > 0.3f ? 1 : 0;
            int obstacleHeight = Random.value > 0.3f ? 1 : 0;

            if (nodes[xObstaclePosition, yObstaclePosition].IsObstructed == false)
            {
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
