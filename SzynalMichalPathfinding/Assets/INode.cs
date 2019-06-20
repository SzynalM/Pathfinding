using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    float XPosition { get; set; }
    float YPosition { get; set; }
    List<Vector2> Neighbours { get; set; }
    bool IsObstructed { get; set; }
    Color color { get; set; }
    SpriteRenderer spriteRenderer { get; set; }
}
