using UnityEngine;

public interface IFactory
{
    void Create(GameObject prefab, Vector2 position, int amount, Transform parent = null);
}
