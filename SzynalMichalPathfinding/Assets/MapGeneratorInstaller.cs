using UnityEngine;
using Zenject;
using Pathfinding;
using MapGeneration;
using UI;

public class MapGeneratorInstaller : MonoInstaller<MapGeneratorInstaller>
{
    [SerializeField]
    private DijkstraNode dijkstraNodePrefab;
    [SerializeField]
    private AStarNode astarNodePrefab;
    [SerializeField]
    private ShowPopup showPopup;
    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private NodeFactory nodeFactory;
    [SerializeField]
    private GameDataContainer gameDataContainer;
    [SerializeField]
    private Material obstructedMaterial;

    public override void InstallBindings()
    {
        DeclarSignals();
        BindSignals();
        BindTypes();
    }

    private void DeclarSignals()
    {
        Container.DeclareSignal<MapGeneratedSignal>();
        Container.DeclareSignal<GenerationDataValidatedSignal>();
        Container.DeclareSignal<ErrorOccuredSignal>();
    }

    private void BindSignals()
    {
        Container.BindSignal<GenerationDataValidatedSignal>().ToMethod(mapGenerator.GenerateMap);
        Container.BindSignal<ErrorOccuredSignal>().ToMethod(showPopup.ShowMessage);
        Container.BindSignal<MapGeneratedSignal>().ToMethod(gameDataContainer.SetStartAndEndPositions);
    }

    private void BindTypes()
    {
        Container.Bind<DijkstraNode>().FromInstance(dijkstraNodePrefab).WhenInjectedInto<MapGenerator>();
        Container.Bind<AStarNode>().FromInstance(astarNodePrefab).WhenInjectedInto<MapGenerator>();
        Container.Bind<MapGenerator>().FromInstance(mapGenerator);
        Container.Bind<IFactory<GameObject, Transform, Vector2, Node>>().FromInstance(nodeFactory);
    }
}