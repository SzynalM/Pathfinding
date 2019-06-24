using UnityEngine;

public class PathfindingRunner
{
    private IPathfinder algorithm;

    public PathfindingRunner(IPathfinder _algorithm)
    {
        algorithm = _algorithm;
    }

    public void RunAlgorithm(Vector2 startPosition, Vector2 endPosition)
    {
        algorithm.FindPath(startPosition, endPosition);
    }
}