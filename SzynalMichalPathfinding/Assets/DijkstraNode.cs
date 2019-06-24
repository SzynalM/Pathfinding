using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), System.Serializable]
public class DijkstraNode : MonoBehaviour, INode
{
    [SerializeField]
    private Sprite startSprite;
    [SerializeField]
    private Sprite endSprite;
    [SerializeField]
    private GameObject lineRendererChildGameObject;
    public int DistanceFromStart { get; set; }
    public Vector2 Position { get; set; }
    private List<INode> neighbours { get; set; }
    public List<INode> Neighbours { get { return neighbours; } set { neighbours = value; CreateLines(); } }
    private bool isObstructed;
    public bool IsObstructed { get { return isObstructed; } set { isObstructed = value; ChangeColor(); } }
    public SpriteRenderer SpriteRenderer { get; set; }
    public INode Parent { get; set; }
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

    public List<LineRenderer> lines;

    private void OnEnable()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void CreateLines()
    {
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

    private void ChangeColor()
    {
        if (isObstructed == true)
        {
            SpriteRenderer.color = Color.red;
        }
        else
        {
            SpriteRenderer.color = Color.white;
        }
    }
}