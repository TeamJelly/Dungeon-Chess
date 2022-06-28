using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShaderControlTest : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color Color1, Color2;
    public float Speed = 1, Offset;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        //_renderer.material.DOColor(Color1, 0.9f).SetLoops(-1);

        SetBlink(Color1, 1);
        //_renderer.material.DOColor(Color1, 1f).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnValidate()
    {
        if (!Application.isPlaying) return;
        SetBorderColor(1f);
    }
    public void SetBorderColor(float duration)
    {
        _renderer.material.DOPause();
        //Color1 = color;
        _renderer.material.DOColor(Color1, duration);
    }

    public void SetBlink(Color color, float duration)
    {
        _renderer.material.DOPause();
        _renderer.material.color = Color.black;
        _renderer.material.DOColor(color, duration).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);
    }
    // void Update()
    // {
    //     // Get the current value of the material properties in the renderer.
    //     _renderer.GetPropertyBlock(_propBlock);

    //     // Assign our new value.
    //     _propBlock.SetColor("_Color", Color.Lerp(Color1, Color2, (Mathf.Sin(Time.time * Speed + Offset) + 1) / 2f));
    //     // Apply the edited values to the renderer.
    //     _renderer.SetPropertyBlock(_propBlock);
    // }
}
