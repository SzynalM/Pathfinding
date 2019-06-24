using UnityEngine;
using Zenject;
using UI;

namespace MapGeneration
{
    public class DataValidation : MonoBehaviour
    {
        private SignalBus signalBus;
        private GameDataContainer gameDataContainer;

        [Inject]
        private void Initialize(SignalBus _signalBus, GameDataContainer _gameDataContainer)
        {
            signalBus = _signalBus;
            gameDataContainer = _gameDataContainer;
        }

        public void Validate(GenerateMapClickedSignal generateMapClickedInfo)
        {
            if (CanMapBeGenerated(gameDataContainer.ObstacleAmount, gameDataContainer.EdgeLength))
            {
                signalBus.Fire<GenerationDataValidatedSignal>();
            }
            else
            {
                signalBus.Fire(new ErrorOccuredSignal { textToDisplay = UI.WarningMessages.obstacleAmountTooHigh });
            }
        }

        private bool CanMapBeGenerated(int amountOfObstacles, int edgeLength)
        {
            return ((edgeLength * edgeLength) - ((amountOfObstacles * 4) + 2)) >= 2;
        }
    } 
}