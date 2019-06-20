using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AStarNode : MonoBehaviour, INode
{
    public float XPosition { get; set; }
    public float YPosition { get; set; }
    private bool isObstructed; 
    public bool IsObstructed { get { return isObstructed; } set { isObstructed = value; ChangeColor(); } }
    public List<Vector2> Neighbours { get; set; }
    public int DistanceFromEndPoint { get; set; }
    public Color color { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ChangeColor()
    {
        spriteRenderer.color = color;
    }
}