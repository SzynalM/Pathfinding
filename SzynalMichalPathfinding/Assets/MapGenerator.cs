using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int m; //amount of obstacles

    [SerializeField]
    private int n; //edge length
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private Transform mapOrigin;
    [SerializeField]
    private Transform nodeContainer;
    [SerializeField]
    private NodeFactory nodeFactory;
    [SerializeField]
    private ObstacleGenerator obstacleGenerator;
    [SerializeField]
    private int amountOfObstacles;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 20;

    private GameObject[,] nodes; 

    public void CreateMap()
    {
        nodeFactory.Create(nodePrefab, mapOrigin.position, n, nodeContainer);
        nodes = nodeFactory.Nodes;
        obstacleGenerator.AddObstacles(nodes, amountOfObstacles);

    }
}