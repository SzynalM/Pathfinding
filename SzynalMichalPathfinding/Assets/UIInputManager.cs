using TMPro;
using UnityEngine;
using Zenject;
using MapGeneration;

namespace UI
{
    public class UIInputManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown algorithmDropdown;

        private SignalBus signalBus;
        private MapGenerator mapGenerator;

        private int edgeLength = 10;
        private bool hasMapBeenLoaded;

        [Inject]
        public void Initialize(SignalBus _signalBus, MapGenerator _mapGenerator)
        {
            signalBus = _signalBus;
            mapGenerator = _mapGenerator;
        }

        public void OnObstacleValueChanged(TMP_InputField inputField)
        {
            signalBus.Fire(new ObstacleValueChangedSignal() { obstacleAmount = int.Parse(inputField.text) });
        }

        public void OnEdgeLengthValueChanged(TMP_InputField inputField)
        {
            edgeLength = int.Parse(inputField.text);
            if (edgeLength < 10)
            {
                signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = WarningMessages.mapTooSmall });
                return;
            }
            signalBus.Fire(new EdgeLengthValueChangedSignal() { edgeLength = edgeLength });
        }

        public void OnAlgorithmValueChanged(TMP_Dropdown dropdown)
        {
            signalBus.Fire(new AlgorithmValueChangedSignal() { algorithmIndex = dropdown.value });
        }

        public void ResetView()
        {
            signalBus.Fire<ResetViewClickedSignal>();
        }

        public void GenerateNewMap()
        {
            algorithmDropdown.interactable = true;
            if (edgeLength < 10)
            {
                signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = WarningMessages.mapTooSmall });
                return;
            }
            signalBus.Fire<GenerateMapClickedSignal>();
        }

        public void FindPath()
        {
            signalBus.Fire<FindPathSignal>();
        }

        public void SaveMap()
        {
            signalBus.Fire(new SaveMapClickedSignal() { nodesToSave = mapGenerator.nodes });
        }

        public void LoadMap()
        {
            hasMapBeenLoaded = true;
            signalBus.Fire<LoadMapClickedSignal>();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
