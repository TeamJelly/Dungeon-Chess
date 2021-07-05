using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectInfo
{
    public string spritePath = "";
    public string Description { get; set; }

    Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            if (spritePath == "")
                sprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
            else if (sprite == null && spritePath != "")
                sprite = Resources.Load<Sprite>(spritePath);
            return sprite;
        }
    }
    public TileBase tilebase;
}
