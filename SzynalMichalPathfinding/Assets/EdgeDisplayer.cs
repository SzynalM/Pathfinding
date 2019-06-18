using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EdgeDisplayer : MonoBehaviour
{
    [SerializeField]
    private NodeFactory nodeFactory;
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

    [ContextMenu("xd")]
    private void GO()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                line.SetPositions(GetNodePositions(nodeFactory.Nodes, 10).ToArray());
            }
        }
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
