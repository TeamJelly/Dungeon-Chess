using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [ReadOnly, SerializeField]
    private SpriteRenderer spriteRenderer;

    public List<Sprite> Sprites = new List<Sprite>();

    [ReadOnly]
    public int animationIndex = 0;

    private void OnValidate() => spriteRenderer = GetComponent<SpriteRenderer>();

    public Sequence GetAnimationSequence()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => animationIndex, value => { spriteRenderer.sprite = Sprites[value]; animationIndex = value; }, Sprites.Count - 1, Sprites.Count * (1 / 30f)));
        sequence.Pause();

        return sequence;
    }
}
