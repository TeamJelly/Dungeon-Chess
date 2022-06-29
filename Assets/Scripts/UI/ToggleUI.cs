using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 토글 UI 스크립트
/// 클릭 이벤트시 isOn 값이 변경된다.
/// </summary>
public class ToggleUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Header("토글 On/Off 값")]
    protected bool isOn = false;

    [Header("토글 On/Off 이벤트")]
    public UnityEvent<bool> OnValueChanged = new UnityEvent<bool>();

    [Header("블록 체크시, 클릭이벤트가 작동하지 않는다.")]
    public bool isBlocked = false;

    // 현재 애니메이션 재생중임을 기록하는 변수
    protected bool isAnimating = false;

    // Unity 클릭 이벤트 콜백함수
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // 애니메이션 도중이거나, 블록처리를 했다면 포인터 클릭 이벤트를 무시한다.
        if (isBlocked || isAnimating)
            return;

        // On 이라면 Off , Off 라면 On
        DoToggle(isOn);

        // 토글 이벤트 호출.
        OnValueChanged.Invoke(isOn);
    }

    /// <summary>
    /// 토글 On 함수. override해서 사용하세요.
    /// </summary>
    public virtual void ToggleOn() => isOn = true;

    /// <summary>
    /// 토글 Off 함수. override해서 사용하세요.
    /// </summary>
    public virtual void ToggleOff() => isOn = false;

    /// <summary>
    /// 반대의 상태로 만들어 준다.
    /// </summary>
    /// <param name="value"></param>
    public virtual void DoToggle(bool value)
    {
        // On 이라면, Off로
        if (value) ToggleOff();
        // Off 라면, On으로 전환한다.
        else ToggleOn();
    }
}