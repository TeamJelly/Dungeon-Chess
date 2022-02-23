using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static AnimationManager instance;

    // 인스턴스 등록
    private void Awake()
    {
        instance = this;
    }

    // 애니메이션 대기 큐
    public Queue<Sequence> WaitingQueue = new Queue<Sequence>();

    // public Queue<Sequence> UIAnimationQueue;
    // public Queue<Sequence> SkillAnimationQueue;

    // 현재 재생중인 시퀀스
    public bool isNowPlaying = false;

    // 애니메이션 예약
    public static void Reserve(Sequence sequence)
    {
        instance.WaitingQueue.Enqueue(sequence);

        // 현재 재생중인게 없으면 애니메이션 루틴 재생
        if (instance.isNowPlaying == false)
            instance.StartCoroutine(instance.AnimationRoutine());
    }

    // 애니메이션 재생 코루틴
    // 애니메이션 큐를 모두 해소할때까지 루프하면서 재생
    IEnumerator AnimationRoutine()
    {
        instance.isNowPlaying = true;

        // 현재 대기중인 애니메이션이 하나 이상존재함
        while (WaitingQueue.Count > 0)
        {
            Sequence sequence = WaitingQueue.Dequeue();
            sequence.Play();
            yield return sequence.WaitForCompletion();
        }

        instance.isNowPlaying = false;
    }
}