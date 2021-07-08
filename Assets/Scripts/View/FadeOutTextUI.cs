using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View
{
    public class FadeOutTextUI : MonoBehaviour
    {
        public static FadeOutTextUI instance;
        public GameObject prefab;

        public bool isCoroutineRunning = false;

        public class FadeOutText
        {
            public Vector2Int position;
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

            if (instance.isCoroutineRunning == false)
                instance.StartCoroutine(instance.StartTextCoroutine());
        }

        public static void MakeText(Vector2Int position, string text, Color color)
        {
            MakeText(new FadeOutText() { position = position, text = text, color = color });
        }

        IEnumerator StartTextCoroutine()
        {
            isCoroutineRunning = true;
            
            while (WaitingQueue.Count > 0)
            {
                FadeOutText line = WaitingQueue.Dequeue();
                GameObject gameObject = Instantiate(instance.prefab);
                gameObject.GetComponentInChildren<TextMeshPro>().text = line.text;
                gameObject.GetComponentInChildren<TextMeshPro>().color = line.color;
                gameObject.transform.position = new Vector3(line.position.x, line.position.y);
                yield return new WaitForSeconds(1f);
            }

            isCoroutineRunning = false;
        }
    }
}

