using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class AsyncTest : MonoBehaviour
{
    // Start is called before the first frame update

    async void Start()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));

        await transform.DOMoveX(10, 3).AsyncWaitForCompletion();

        Debug.Log("큐브가 10칸 오른쪽으로 이동함");

        await myCoroutine();

        Debug.Log("큐브가 제자리로 이동함");

    }

    public IEnumerator myCoroutine()
    {
        yield return transform.DOMoveX(0, 3).AsyncWaitForCompletion().AsIEnumerator();

        yield return null;
    }
}
