using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Pixel Perfect Outline/Sprite Outline")]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    [Serializable]
    struct Directions
    {
        public bool top;
        public bool bottom;
        public bool left;
        public bool right;

        public Directions(bool top, bool bottom, bool left, bool right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
    }

    [SerializeField]
    [HideInInspector]
    Material material;

    [SerializeField]
    Color outlineColor = Color.white;

    [SerializeField]
    Directions directions = new Directions(true, true, true, true);

    Color currentOutlineColor;
    Rect currentRect;
    Vector2 currentPivot;
    float currentPixelsPerUnit;
    Directions currentDirections;

    SpriteRenderer spriteRenderer;

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
            UpdateProperties();
        }
    }

    void Reset()
    {
        spriteRenderer.material = material;
        UpdateProperties();
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateProperties();
    }

    void LateUpdate()
    {
        UpdateProperties();
    }

    void UpdateProperties()
    {
        Rect spriteRect = spriteRenderer.sprite.rect;
        Vector2 pivot = spriteRenderer.sprite.pivot;
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;

        if (outlineColor == currentOutlineColor && spriteRect == currentRect && pivot == currentPivot &&
            Mathf.Approximately(pixelsPerUnit, currentPixelsPerUnit) && directions.Equals(currentDirections))
            return;

        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(properties);

        Vector4 vector = new Vector4(spriteRect.x, spriteRect.y, spriteRect.width, spriteRect.height);
        properties.SetVector("_RectPosSize", vector);
        properties.SetVector("_Pivot", pivot);
        properties.SetFloat("_PixelsPerUnit", pixelsPerUnit);
        properties.SetColor("_OutlineColor", enabled ? OutlineColor : Color.clear);

        properties.SetFloat("_Top", directions.top ? 1 : 0);
        properties.SetFloat("_Bottom", directions.bottom ? 1 : 0);
        properties.SetFloat("_Left", directions.left ? 1 : 0);
        properties.SetFloat("_Right", directions.right ? 1 : 0);

        spriteRenderer.SetPropertyBlock(properties);

        currentRect = spriteRect;
        currentPivot = pivot;
        currentPixelsPerUnit = pixelsPerUnit;
        currentOutlineColor = outlineColor;
        currentDirections = directions;
    }

    void OnDrawGizmosSelected()
    {
        UpdateProperties();
    }
}