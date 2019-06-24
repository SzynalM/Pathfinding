using UnityEngine;
using Zenject;

public class NodeFactory : MonoBehaviour, IFactory<GameObject, Transform, Vector2, INode>
{
    public INode Create(GameObject prefab, Transform parent, Vector2 position)
    {
        if(prefab.GetComponent<INode>() == null)
        {
            Debug.LogError("Invalid node prefab");
            return null;
        }
        GameObject nodeGameObject = Instantiate(prefab, position, Quaternion.identity, parent);
        nodeGameObject.name = "Node";
        INode node = nodeGameObject.GetComponent<INode>();
        node.Position = position - (Vector2)parent.position;
        return node;
    }
}
