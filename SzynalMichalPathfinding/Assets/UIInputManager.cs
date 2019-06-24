using TMPro;
using UnityEngine;
using Zenject;

public class UIInputManager : MonoBehaviour
{
    private SignalBus signalBus;
    private MapGenerator mapGenerator;

    private int edgeLength = 10;
    private int amountOfObstacles = 10;
    private int algorithmIndex = 0;

    [Inject]
    public void Initialize(SignalBus _signalBus, MapGenerator _mapGenerator)
    {
        signalBus = _signalBus;
        mapGenerator = _mapGenerator;
    }

    public void OnObstacleValueChanged(TMP_InputField inputField)
    {
        amountOfObstacles = int.Parse(inputField.text);
    }

    public void OnEdgeLengthValueChanged(TMP_InputField inputField)
    {
        edgeLength = int.Parse(inputField.text);
        if (edgeLength < 10)
        {
            signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = WarningMessages.mapTooLittle });
            return;
        }
    }

    public void OnAlgorithmValueChanged(TMP_Dropdown dropdown)
    {
        algorithmIndex = dropdown.value;
    }

    public void ResetView()
    {
        signalBus.Fire<ResetViewClickedSignal>();
    }

    public void GenerateNewMap()
    {
        if (edgeLength < 10)
        {
            signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = WarningMessages.mapTooLittle });
            return;
        }
        signalBus.Fire(new GenerateMapClickedSignal() { algorithmIndex = algorithmIndex, amountOfObstacles = amountOfObstacles, edgeLength = edgeLength });
    }

    public void SaveMap()
    {
        signalBus.Fire(new SaveMapClickedSignal() { nodesToSave = mapGenerator.nodes });
    }

    public void LoadMap()
    {
        signalBus.Fire(new LoadMapClickedSignal() { algorithmIndex = algorithmIndex });
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
