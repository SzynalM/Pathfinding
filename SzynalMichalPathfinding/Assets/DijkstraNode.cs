using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DijkstraNode : MonoBehaviour, INode
{
    public int DistanceFromStart { get; set; }
    public Vector2 Position { get; set; }
    public List<INode> Neighbours { get; set; }
    private bool isObstructed;
    public bool IsObstructed { get { return isObstructed; } set { isObstructed = value; ChangeColor(); } }
    public Color Color { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public DijkstraNode Parent;

    private void OnEnable()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ChangeColor()
    {
        SpriteRenderer.color = Color;
    }
}