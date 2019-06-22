using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    bool IsStartPoint { get; set; }
    bool IsEndPoint { get; set; }
    Vector2 Position { get; set; }
    List<INode> Neighbours { get; set; }
    bool IsObstructed { get; set; }
    SpriteRenderer SpriteRenderer { get; set; }
    INode Parent { get; set; }
}
