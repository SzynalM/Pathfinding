using UnityEngine;
using Zenject;

public class MapGeneratorInstaller : MonoInstaller<MapGeneratorInstaller>
{
    [SerializeField]
    private DijkstraNode dijkstraNodePrefab;
    [SerializeField]
    private AStarNode astarNodePrefab;
    [SerializeField]
    private DijkstraPathfinding dijkstraPathfinding;
    [SerializeField]
    private AStarPathfinding aStarPathfinding;
    [SerializeField]
    private ShowPopup showPopup;
    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private NodeFactory nodeFactory;
    private int chosenAlgorithmIndex;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private void SetAlgorithmIndex(GenerationDataValidatedSignal generationDataValidated)
    {
        chosenAlgorithmIndex = generationDataValidated.algorithmIndex;
    }

    private void SetAlgorithmIndex(int algorithIndex)
    {
        chosenAlgorithmIndex = algorithIndex;
    }

    private void RunPathfindingAlgorithm(MapGeneratedSignal mapGeneratedSignalInfo)
    {
        startPosition = mapGeneratedSignalInfo.starPosition;
        endPosition = mapGeneratedSignalInfo.endPosition;
        PathfindingRunner pathfindingRunner = null;
        if (chosenAlgorithmIndex == 0)
        {
            pathfindingRunner = new PathfindingRunner(dijkstraPathfinding);
        }
        else if (chosenAlgorithmIndex == 1)
        {
            pathfindingRunner = new PathfindingRunner(aStarPathfinding);
        }
        pathfindingRunner.RunAlgorithm(startPosition, endPosition);
    }

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
        Container.BindSignal<GenerationDataValidatedSignal>().ToMethod((x) => { chosenAlgorithmIndex = x.algorithmIndex; mapGenerator.GenerateMap(x); });
        Container.BindSignal<ErrorOccuredSignal>().ToMethod(showPopup.ShowMessage);
        Container.BindSignal<MapGeneratedSignal>().ToMethod(RunPathfindingAlgorithm);
        Container.BindSignal<LoadMapClickedSignal>().ToMethod((x) => SetAlgorithmIndex(x.algorithmIndex));
    }

    private void BindTypes()
    {
        Container.Bind<DijkstraNode>().FromInstance(dijkstraNodePrefab).WhenInjectedInto<MapGenerator>();
        Container.Bind<AStarNode>().FromInstance(astarNodePrefab).WhenInjectedInto<MapGenerator>();
        Container.Bind<MapGenerator>().FromInstance(mapGenerator);
        Container.Bind<IFactory<GameObject, Transform, Vector2, INode>>().FromInstance(nodeFactory);
    }
}