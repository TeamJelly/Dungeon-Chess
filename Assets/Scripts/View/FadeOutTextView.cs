using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Model;

namespace View
{
    public class FadeOutTextView : MonoBehaviour
    {
        public static FadeOutTextView instance;
        public GameObject prefab;

        public bool isCoroutineRunning = false;

        public class FadeOutText
        {
            public Unit unit;
            public string text;
            public Color color;
        }

        public Queue<FadeOutText> WaitingQueue = new Queue<FadeOutText>();

        private void Awake()
        {
            instance = this;
            prefab = Resources.Load<GameObject>("Prefabs/UI/FadeOutText");
        }

        public static void MakeText(FadeOutText fadeOutText)
        {
            instance.WaitingQueue.Enqueue(fadeOutText);
            PlayText();
        }

        public static void MakeText(Unit unit, string text, Color color)
        {
            MakeText(new FadeOutText() { unit = unit, text = text, color = color });
        }

        public static void PlayText()
        {
            if (instance.isCoroutineRunning == false)
                instance.StartCoroutine(instance.TextCoroutine());
        }

        public static void StopText()
        {
            if (instance.isCoroutineRunning == true)
                instance.StopCoroutine(instance.TextCoroutine());
        }

        IEnumerator TextCoroutine()
        {
            isCoroutineRunning = true;

            while (WaitingQueue.Count > 0)
            {
                FadeOutText line = WaitingQueue.Dequeue();
                GameObject gameObject = Instantiate(instance.prefab);
                gameObject.GetComponentInChildren<TextMeshPro>().text = line.text;
                gameObject.GetComponentInChildren<TextMeshPro>().color = line.color;
                gameObject.transform.position = new Vector3(line.unit.Position.x, line.unit.Position.y + 1);
                yield return new WaitForSeconds(1f);
            }

            isCoroutineRunning = false;
        }
    }
}