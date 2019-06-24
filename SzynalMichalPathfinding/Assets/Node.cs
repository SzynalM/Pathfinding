using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    [System.Serializable, RequireComponent(typeof(SpriteRenderer))]
    public class Node : MonoBehaviour
    {
        [SerializeField]
        protected GameObject lineRendererChildGameObject;
        [SerializeField]
        protected Sprite startSprite;
        [SerializeField]
        protected Sprite endSprite;

        private List<Node> neighbours { get; set; }
        public List<Node> Neighbours { get { return neighbours; } set { neighbours = value; CreateLines(); } }
        private bool isObstructed;
        public bool IsObstructed { get { return isObstructed; } set { isObstructed = value; if (isObstructed == true) { ChangeColor(Color.black); } } }

        protected SpriteRenderer SpriteRenderer { get; set; }
        public List<LineRenderer> lines;
        public Vector2 Position { get; set; }
        public Node Parent { get; set; }

        private bool isStartPoint;
        private bool isEndPoint;
        public bool IsStartPoint
        {
            get
            {
                return isStartPoint;
            }
            set
            {
                if (value == true)
                {
                    IsEndPoint = false;
                    ChangeSprite(startSprite);
                }
                isStartPoint = value;
            }
        }

        public bool IsEndPoint
        {
            get
            {
                return isEndPoint;
            }
            set
            {
                if (value == true)
                {
                    IsStartPoint = false;
                    ChangeSprite(endSprite);
                }
                isEndPoint = value;
            }
        }

        private void OnEnable()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void CreateLines()
        {
            lines = new List<LineRenderer>();
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < neighbours.Count; i++)
            {
                GameObject lineRendererChild = Instantiate(lineRendererChildGameObject, transform);
                lineRendererChild.name = "LineRenderer";
                LineRenderer renderer = lineRendererChild.GetComponent<LineRenderer>();
                renderer.SetPosition(0, lineRendererChild.transform.localPosition);
                renderer.SetPosition(1, (neighbours[i].Position - Position) * 10);
                lines.Add(renderer);
            }
        }

        private void ChangeSprite(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
        }

        public void ChangeColor(Color color)
        {
            SpriteRenderer.color = color;
        }
    }

}