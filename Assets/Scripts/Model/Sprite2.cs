using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite2 
{
    private Sprite sprite;

    public Sprite Sprite { get => sprite; set => sprite = value; }

    Sprite2(int spriteNum, Color inlineColor, Color outlineColor)
    {
        InlineColor = inlineColor;
        OutlineColor = outlineColor;
    }

    public Color InlineColor {get; set;}
    public Color OutlineColor {get; set;}

}
