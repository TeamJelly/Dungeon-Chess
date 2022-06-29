using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// Toggle On/Off시 오브젝트를 축소/확장 UI Animation 스크립트
/// </summary>
public class ExpandUI : ToggleUI
{
    [Header("확장시킬 RectTransfrom을 등록해주세요")]
    public List<RectTransform> rectTransforms;

    [Header("RectTransform이 확장할 크기를 등록해주세요")]
    public List<Vector2> deltaSizes;

    [Header("애니메이션 진행시간")]
    public float animationTime = 0.25f;

    /// <summary>
    /// 시작시 토글이 켜져있다면(펼쳐져 있다면), 토글을 끄고 축소시킵니다.
    /// </summary>
    private void Start()
    {
        if (isOn)
        {
            isOn = false;
            for (int i = 0; i < rectTransforms.Count && i < deltaSizes.Count; i++)
                rectTransforms[i].sizeDelta = rectTransforms[i].sizeDelta - deltaSizes[i];
        }
    }

    public override void ToggleOff()
    {
        base.ToggleOff();

        Sequence sequence = DOTween.Sequence().OnStart(() => isAnimating = true).OnComplete(() => isAnimating = false);

        for (int i = 0; i < rectTransforms.Count && i < deltaSizes.Count; i++)
            sequence.Join(rectTransforms[i].DOSizeDelta(rectTransforms[i].sizeDelta - deltaSizes[i], animationTime));
    }

    public override void ToggleOn()
    {
        base.ToggleOn();

        Sequence sequence = DOTween.Sequence().OnStart(() => isAnimating = true).OnComplete(() => isAnimating = false);

        for (int i = 0; i < rectTransforms.Count && i < deltaSizes.Count; i++)
            sequence.Join(rectTransforms[i].DOSizeDelta(rectTransforms[i].sizeDelta + deltaSizes[i], animationTime));
    }
}