using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UI;

namespace Pathfinding
{
    public class FinalPathDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Material finalPathNodeMaterial;
        private SignalBus signalBus;

        [Inject]
        private void Initialize(SignalBus _signalBus)
        {
            signalBus = _signalBus;
        }

        public void DisplayFinalPath(PathFoundSignal pathFoundSignalInfo)
        {
            List<Node> FinalPath = new List<Node>();
            FinalPath.Add(pathFoundSignalInfo.endNode);
            Node currentNode = pathFoundSignalInfo.endNode;
            Vector2 previousPosition = new Vector2();
            while (currentNode != pathFoundSignalInfo.startingNode)
            {
                if (currentNode == null)
                {
                    signalBus.Fire(new ErrorOccuredSignal() { textToDisplay = UI.WarningMessages.noPathFound });
                    return;
                }
                currentNode = currentNode.Parent as Node;
                FinalPath.Add(currentNode);
            }
            FinalPath.Reverse();
            FinalPath[0].ChangeColor(Color.green);
            for (int i = 1; i < FinalPath.Count; i++)
            {
                Node node = FinalPath[i];
                previousPosition = FinalPath[i - 1].Position;
                node.ChangeColor(Color.green);
                LineRenderer currentRenderer = node.lines.Where(x => (previousPosition - node.Position) * 10 == (Vector2)x.GetPosition(1)).First();
                currentRenderer.startColor = Color.black;
                currentRenderer.endColor = Color.black;
                currentRenderer.startWidth = .2f;
                currentRenderer.endWidth = .2f;
                currentRenderer.sortingOrder = -1;
            }
        }
    } 
}