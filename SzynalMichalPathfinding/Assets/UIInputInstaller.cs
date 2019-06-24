using Zenject;
using UnityEngine;
using CameraMovement;
using MapGeneration;
using DataHandling;

namespace UI
{
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
        [SerializeField]
        private GameDataContainer gameDataContainer;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<GameDataContainer>().FromInstance(gameDataContainer);
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

            Container.BindSignal<ObstacleValueChangedSignal>().ToMethod(gameDataContainer.SetObstacleAmount);
            Container.BindSignal<EdgeLengthValueChangedSignal>().ToMethod(gameDataContainer.SetEdgeLength);
            Container.BindSignal<AlgorithmValueChangedSignal>().ToMethod(gameDataContainer.SetAlgorithmIndex);
            Container.BindSignal<ResetViewClickedSignal>().ToMethod(cameraMovementController.ResetView);
        }
    } 
}