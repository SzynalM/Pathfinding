using UnityEngine;
using Zenject;
using Pathfinding;

namespace MapGeneration
{
    public class NodeFactory : MonoBehaviour, IFactory<GameObject, Transform, Vector2, Node>
    {
        public Node Create(GameObject prefab, Transform parent, Vector2 position)
        {
            if (prefab.GetComponent<Node>() == null)
            {
                Debug.LogError("Invalid node prefab");
                return null;
            }
            GameObject nodeGameObject = Instantiate(prefab, position, Quaternion.identity, parent);
            nodeGameObject.name = "Node";
            Node node = nodeGameObject.GetComponent<Node>();
            node.Position = position - (Vector2)parent.position;
            return node;
        }
    } 
}
