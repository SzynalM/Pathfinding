using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EdgeDisplayer : MonoBehaviour
{
    [SerializeField]
    private MapGenerator mapGenerator;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 100;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    public void ShowLines()
    {
        line.SetPositions(GetNodePositions(GetNodes(mapGenerator.nodes, mapGenerator.edgeLength), mapGenerator.edgeLength).ToArray());

    }

    private GameObject[,] GetNodes(INode[,] originalNodes, int edgeLength)
    {
        GameObject[,] nodes = new GameObject[edgeLength,edgeLength];

        for(int i = 0; i < edgeLength; i++)
        {
            for(int j = 0; j < edgeLength; j++)
            {
                nodes[i, j] = originalNodes[i, j].SpriteRenderer.gameObject;
            }
        }
        return nodes;
    }

    private List<Vector3> GetNodePositions(GameObject[,] nodes, int length)
    {
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                positions.Add(nodes[i,j].transform.position);
            }
        }
        return positions;
    }
}
