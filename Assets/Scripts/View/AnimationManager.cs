using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Model;
using Model.Managers;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static AnimationManager instance;
    private void Awake() => instance = this;

    [Header("Animation Clips")]
    public List<GameObject> animationClips;
    public List<string> animationNames;


    public static void ReserveFadeTextClips(Unit unit, string text, Color color) => Reserve(MakeFadeTextClips(unit, text, color));

    public static void ReserveAnimationClips(string name, Vector2Int position) => Reserve(MakeAnimationClips(name, position));

    public static Sequence MakeFadeTextClips(Unit unit, string text, Color color)
    {
        GameObject gameObject = Instantiate(instance.animationClips[instance.animationNames.IndexOf("FadeOutText")], new Vector3(unit.Position.x, unit.Position.y + 1, -2), Quaternion.identity);

        gameObject.SetActive(false);

        TextMeshPro textMeshProUGUI = gameObject.GetComponentInChildren<TextMeshPro>();
        Sequence sequence = DOTween.Sequence();

        textMeshProUGUI.text = text;
        textMeshProUGUI.color = color;

        sequence
        .Append(textMeshProUGUI.DOFade(0, GameManager.AnimationDelaySpeed + 0.5f).SetEase(Ease.InQuint).From(1))
        .Join(gameObject.transform.DOLocalMoveY(0.5f, GameManager.AnimationDelaySpeed + 0.5f).SetRelative())
        .OnStart(() => gameObject.SetActive(true)).OnComplete(() => GameObject.Destroy(gameObject))
        .Pause();

        return sequence;
    }

    public static Sequence MakeAnimationClips(string name, Vector2Int position)
    {
        GameObject gameObject = Instantiate(instance.animationClips[instance.animationNames.IndexOf(name)], new Vector3(position.x, position.y, -2), Quaternion.identity);
        gameObject.SetActive(false);

        Sequence sequence = DOTween.Sequence();

        SpriteAnimation spriteAnimation = gameObject.GetComponent<SpriteAnimation>();
        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();

        // Category 2 : SpriteAnimation
        if (spriteAnimation != null)
        {
            sequence = spriteAnimation.GetAnimationSequence()
            .OnStart(() => gameObject.SetActive(true))
            .OnComplete(() => GameObject.Destroy(gameObject));
        }
        // Category 3 : ParticleAnimation
        else if (particleSystem != null)
        {
            sequence.Append(DOVirtual.DelayedCall(particleSystem.main.startLifetime.constant, () => GameObject.Destroy(gameObject)))
            .OnStart(() => gameObject.SetActive(true))
            .OnComplete(() => GameObject.Destroy(gameObject));
        }
        sequence.Pause();

        return sequence;
    }

    // 애니메이션 대기 큐
    public Queue<Sequence> WaitingQueue = new Queue<Sequence>();
    // public Queue<Sequence> UIAnimationQueue;
    // public Queue<Sequence> SkillAnimationQueue;

    // 현재 재생중인 시퀀스
    public static bool isNowPlaying = false;

    // 애니메이션 예약
    public static void Reserve(Sequence sequence)
    {
        sequence.Pause();
        instance.WaitingQueue.Enqueue(sequence);

        // 현재 재생중인게 없으면 애니메이션 루틴 재생
        if (isNowPlaying == false)
            instance.StartCoroutine(instance.AnimationRoutine());
    }

    // 애니메이션 재생 코루틴
    // 애니메이션 큐를 모두 해소할때까지 루프하면서 재생
    IEnumerator AnimationRoutine()
    {
        isNowPlaying = true;

        // 현재 대기중인 애니메이션이 하나 이상존재함
        while (WaitingQueue.Count > 0)
        {
            Sequence sequence = WaitingQueue.Dequeue();
            sequence.Play();
            yield return sequence.WaitForCompletion();
        }

        isNowPlaying = false;
    }

    public static void OnAnimationComplete(UnityAction unityAction)
    {
        instance.StartCoroutine(OnAnimationCompleteRoutine(unityAction));
    }

    private static IEnumerator OnAnimationCompleteRoutine(UnityAction unityAction)
    {
        yield return new WaitWhile(() => AnimationManager.isNowPlaying);
        unityAction.Invoke();
    }
}