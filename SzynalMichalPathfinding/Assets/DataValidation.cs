using Zenject;
using UnityEngine;

public class DataValidation : MonoBehaviour
{
    private SignalBus signalBus;

    [Inject]
    private void Initialize(SignalBus _signalBus)
    {
        signalBus = _signalBus;
    }

    public void Validate(GenerateMapClickedSignal generateMapClickedInfo)
    {
        if (CanMapBeGenerated(generateMapClickedInfo.amountOfObstacles, generateMapClickedInfo.edgeLength))
        {
            signalBus.Fire(new GenerationDataValidatedSignal()
            {
                algorithmIndex = generateMapClickedInfo.algorithmIndex,
                amountOfObstacles = generateMapClickedInfo.amountOfObstacles,
                edgeLength = generateMapClickedInfo.edgeLength
            });
        }
        else
        {
            signalBus.Fire(new ErrorOccuredSignal { textToDisplay = WarningMessages.obstacleAmountTooHigh});
        }
    }

    private bool CanMapBeGenerated(int amountOfObstacles, int edgeLength)
    {
        return ((edgeLength * edgeLength) - ((amountOfObstacles * 4) + 2)) >= 2;
    }
}