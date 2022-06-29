using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 팝업 UI 활성화/비활성화 Animation 스크립트
/// </summary>
public class PopUpUI : MonoBehaviour
{
    [Header("PopUp UI Blocking 백패널을 등록해주세요")]
    public Image backPanel;

    // 팝업 UI RectTransform
    private RectTransform rectTransform;

    // 팝업 UI 초기 Scale 값
    private Vector3 originScale;

    // 팝업 UI 초기 position 값
    private Vector2 originPosition;

    [Header("애니메이션 재생 시간")]
    public float animationTime = 0.25f;

    /// <summary>
    /// 시작시 오브젝트를 비활성화
    /// </summary>
    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originScale = rectTransform.localScale;
        originPosition = rectTransform.localPosition;

        gameObject.SetActive(false);
        backPanel?.gameObject.SetActive(false);
    }

    /// <summary>
    /// 팝업UI 보이기 애니메이션 함수
    /// </summary>
    public void Show()
    {
        rectTransform.localPosition = Vector3.zero;
        rectTransform.DOScale(originScale, animationTime).From(Vector3.zero)
        .OnStart(() =>
        {
            gameObject.SetActive(true);
            backPanel?.gameObject.SetActive(true);
        })
        .SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 팝업UI 숨기기 애니메이션 함수
    /// </summary>
    public void Hide()
    {
        if (gameObject.activeSelf == false)
            return;

        rectTransform.DOScale(Vector3.zero, animationTime).From(originScale)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            rectTransform.localScale = originScale;
            rectTransform.localPosition = originPosition;
            gameObject.SetActive(false);
            backPanel?.gameObject.SetActive(false);
        });
    }
}