using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2 Position { get; set; }
    public bool IsObstructed { get; set; }
    public INode Parent { get; set; }
    public bool IsStartPoint { get; set; }
    public bool IsEndPoint { get; set; }
}
