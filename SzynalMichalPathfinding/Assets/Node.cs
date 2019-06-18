using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color obstructedColor;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public bool IsObstructed
    {
        get
        {
            return IsObstructed;
        }
        set
        {
            if(value == true)
            {
                spriteRenderer.color = obstructedColor;
            }
            else
            {
                spriteRenderer.color = defaultColor;
            }
        }
    }
}
