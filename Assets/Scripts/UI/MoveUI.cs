using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Toggle On/Off시 오브젝트를 이동/복귀시키는 UI Animation 스크립트
/// </summary>
public class MoveUI : ToggleUI
{
    [Header("이동시킬 RectTransform을 등록해주세요")]
    public List<RectTransform> rectTransforms;

    [Header("RectTransform이 이동할 거리를 등록해주세요")]
    public List<Vector3> deltaPositions;

    [Header("애니메이션 진행시간")]
    public float animationTime = 0.25f;

    [Header("애니메이션 종류")]
    public Ease ease = Ease.Unset;

    /// <summary>
    /// 시작시 토글이 켜져있다면, 토글을 끄고 복귀시킵니다.
    /// </summary>
    private void Start()
    {
        if (isOn)
        {
            isOn = false;
            for (int i = 0; i < rectTransforms.Count && i < deltaPositions.Count; i++)
                rectTransforms[i].localPosition = rectTransforms[i].localPosition - deltaPositions[i];
        }
    }

    public override void ToggleOff()
    {
        base.ToggleOff();

        Sequence sequence = DOTween.Sequence().OnStart(() => isAnimating = true).OnComplete(() => isAnimating = false).SetEase(ease);

        for (int i = 0; i < rectTransforms.Count && i < deltaPositions.Count; i++)
            sequence.Join(rectTransforms[i].DOLocalMove(rectTransforms[i].localPosition - deltaPositions[i], animationTime));
    }

    public override void ToggleOn()
    {
        base.ToggleOn();

        Sequence sequence = DOTween.Sequence().OnStart(() => isAnimating = true).OnComplete(() => isAnimating = false).SetEase(ease);

        for (int i = 0; i < rectTransforms.Count && i < deltaPositions.Count; i++)
            sequence.Join(rectTransforms[i].DOLocalMove(rectTransforms[i].localPosition + deltaPositions[i], animationTime));
    }
}
