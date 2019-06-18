using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private int maxObstacles = 10;
    [SerializeField]
    private float defaultObstacleProbability = 0.1f;
    private float obstacleProbability;

    public void AddObstacles(GameObject[,] nodes, int amount)
    {
        int arrayLength = nodes.GetLength(0);
        Debug.Log("Length: " + arrayLength);
        obstacleProbability = defaultObstacleProbability;
        for (int i = 0; i < arrayLength; i++)
        {
            for (int j = 0; j < arrayLength; j++)
            {
                if (amount < maxObstacles)
                {
                    Debug.Log("J " + j);
                    if (Random.value < obstacleProbability)
                    {
                        nodes[i, j].GetComponent<Node>().IsObstructed = true;
                        amount++;
                    }
                    if (ApproachingEndOfNodes(arrayLength - 1, amount, i, j))
                    {
                        obstacleProbability = 1;
                        Debug.Log("Approaching");

                    }
                }
            }
        }
    }

    private bool ApproachingEndOfNodes(int maximumIndex, int amount, int i, int j)
    {
        return i == 9 && j > 4 && amount < maxObstacles;
    }
}