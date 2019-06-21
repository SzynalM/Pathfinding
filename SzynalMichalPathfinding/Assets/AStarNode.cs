using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AStarNode : MonoBehaviour, INode
{
    public float G_Cost { get; set; }
    public float H_Cost { get; set; }
    public float F_Cost { get { return G_Cost + H_Cost; }}
    public AStarNode Parent;

    private bool isObstructed; 
    public bool IsObstructed { get { return isObstructed; } set { isObstructed = value; ChangeColor(); } }

    public List<INode> Neighbours { get; set; }
    public int DistanceFromEndPoint { get; set; }
    public Color Color { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Vector2 Position { get; set; }

    private void OnEnable()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ChangeColor()
    {
        SpriteRenderer.color = Color;
    }
}
