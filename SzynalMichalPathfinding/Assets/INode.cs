using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    Vector2 Position { get; set; }
    List<INode> Neighbours { get; set; }
    bool IsObstructed { get; set; }
    Color Color { get; set; }
    SpriteRenderer SpriteRenderer { get; set; }
}
