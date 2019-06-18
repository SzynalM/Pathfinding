using UnityEngine;
using System.Collections;

public class NodeFactory : MonoBehaviour, IFactory
{
    private GameObject[,] nodes;
    public GameObject[,] Nodes { get { return nodes; } }

    public void Create(GameObject prefab, Vector2 position, int amount, Transform parent)
    {
        nodes = new GameObject[amount, amount];
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < amount; j++)
            {
                nodes[i, j] = (Instantiate(prefab, new Vector2(position.x + i, position.y + j), Quaternion.identity, parent));
            }
        }
    }
}
