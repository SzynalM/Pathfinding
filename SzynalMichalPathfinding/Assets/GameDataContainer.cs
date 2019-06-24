using UnityEngine;
using UI;
using MapGeneration;

public class GameDataContainer : MonoBehaviour
{
    private int edgeLength = 10;
    private int obstacleAmount = 10;
    private int algorithmIndex = 0;
    private Vector2 startPosition;
    private Vector2 endPosition;
    [SerializeField]
    private Material obstructedMaterial;

    public int EdgeLength { get { return edgeLength; } set { edgeLength = value; } }
    public int ObstacleAmount { get { return obstacleAmount; } set { obstacleAmount = value; } }
    public int AlgorithmIndex { get { return algorithmIndex; } set { algorithmIndex = value; } }
    public Vector2 StartPosition { get { return startPosition; } set { startPosition = value; } }
    public Vector2 EndPosition { get { return endPosition; } set { endPosition = value; } }

    public void SetEdgeLength(EdgeLengthValueChangedSignal edgeLengthValueChangedSignalInfo)
    {
        EdgeLength = edgeLengthValueChangedSignalInfo.edgeLength;
    }

    public void SetObstacleAmount(ObstacleValueChangedSignal obstacleValueChangedSignalInfo)
    {
        ObstacleAmount = obstacleValueChangedSignalInfo.obstacleAmount;
    }

    public void SetAlgorithmIndex(AlgorithmValueChangedSignal algorithmValueChangedSignalInfo)
    {
        AlgorithmIndex = algorithmValueChangedSignalInfo.algorithmIndex;
    }

    public void SetStartAndEndPositions(MapGeneratedSignal mapGeneratedSignalInfo)
    {
        StartPosition = mapGeneratedSignalInfo.starPosition;
        EndPosition = mapGeneratedSignalInfo.endPosition;
    }
}