using Zenject;
using UnityEngine;

public class UIInputInstaller : MonoInstaller<UIInputInstaller>
{
    [SerializeField]
    private UIInputManager uIInputManager;
    [SerializeField]
    private CameraMovementController cameraMovementController;
    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private DataValidation dataValidation;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        DeclareSignals();
        BindSignals();
    }

    private void DeclareSignals()
    {
        Container.DeclareSignal<GenerateMapClickedSignal>();
        Container.DeclareSignal<SaveMapClickedSignal>();
        Container.DeclareSignal<LoadMapClickedSignal>();

        Container.DeclareSignal<EdgeLengthValueChangedSignal>();
        Container.DeclareSignal<ObstacleValueChangedSignal>();
        Container.DeclareSignal<AlgorithmValueChangedSignal>();
        Container.DeclareSignal<ResetViewClickedSignal>();
    }

    private void BindSignals()
    {
        Container.BindSignal<GenerateMapClickedSignal>().ToMethod(dataValidation.Validate);
        Container.BindSignal<SaveMapClickedSignal>().ToMethod(new DataSaver().SaveGame);
        Container.BindSignal<LoadMapClickedSignal>().ToMethod(mapGenerator.GenerateMap);

        Container.BindSignal<ObstacleValueChangedSignal>().ToMethod(cameraMovementController.ResetView);
        Container.BindSignal<EdgeLengthValueChangedSignal>().ToMethod(cameraMovementController.ResetView);
        Container.BindSignal<AlgorithmValueChangedSignal>().ToMethod(cameraMovementController.ResetView);
        Container.BindSignal<ResetViewClickedSignal>().ToMethod(cameraMovementController.ResetView);
    }
}