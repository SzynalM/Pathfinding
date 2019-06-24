using UnityEngine;
using Zenject;

namespace Pathfinding
{
    public class PathfindingAlgorithmChooser : MonoBehaviour
    {
        private GameDataContainer gameDataContainer;
        private DijkstraPathfinding dijkstraPathfinding;
        private AStarPathfinding aStarPathfinding;

        [Inject]
        private void Initialize(GameDataContainer _gameDataContainer, DijkstraPathfinding _dijkstraPathfinding, AStarPathfinding _astarPathfinding)
        {
            gameDataContainer = _gameDataContainer;
            dijkstraPathfinding = _dijkstraPathfinding;
            aStarPathfinding = _astarPathfinding;
        }

        public void RunPathfindingAlgorithm()
        {
            if (gameDataContainer.AlgorithmIndex == 0)
            {
                dijkstraPathfinding.FindPath(gameDataContainer.StartPosition, gameDataContainer.EndPosition);
            }
            else if (gameDataContainer.AlgorithmIndex == 1)
            {
                aStarPathfinding.FindPath(gameDataContainer.StartPosition, gameDataContainer.EndPosition);
            }
        }
    } 
}