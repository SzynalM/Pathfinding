using UnityEngine;
using Zenject;
using Pathfinding;
using UI;

public class PathfindingInstaller : MonoInstaller<PathfindingInstaller>
{
    [SerializeField]
    private DijkstraPathfinding dijkstraPathfinding;
    [SerializeField]
    private AStarPathfinding aStarPathfinding;
    [SerializeField]
    private PathfindingAlgorithmChooser pathfindingAlgorithmChooser;
    [SerializeField]
    private FinalPathDisplayer finalPathDisplayer;

    public override void InstallBindings()
    {
        DeclareSignals();
        BindSignals();
        BindTypes();
    }

    private void BindTypes()
    {
        Container.Bind<DijkstraPathfinding>().FromInstance(dijkstraPathfinding).NonLazy();
        Container.Bind<AStarPathfinding>().FromInstance(aStarPathfinding).NonLazy();
        Container.Bind<PathfindingAlgorithmChooser>().FromInstance(pathfindingAlgorithmChooser);
    }

    private void BindSignals()
    {
        Container.BindSignal<FindPathSignal>().ToMethod(pathfindingAlgorithmChooser.RunPathfindingAlgorithm);
        Container.BindSignal<PathFoundSignal>().ToMethod(finalPathDisplayer.DisplayFinalPath);
    }

    private void DeclareSignals()
    {
        Container.DeclareSignal<FindPathSignal>();
        Container.DeclareSignal<PathFoundSignal>();
    }
}
