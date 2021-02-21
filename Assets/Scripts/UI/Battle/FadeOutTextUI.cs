using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI.Battle
{
    public class FadeOutTextUI : MonoBehaviour
    {
        public static FadeOutTextUI instance;

        private GameObject prefab;

        private void Awake()
        {
            instance = this;
            prefab = Resources.Load<GameObject>("Prefabs/UI/FadeOutText");
        }

        public static void MakeText(Vector2Int position, string text, Color color)
        {
            instance.StartCoroutine(instance.MakeTextCoroutine(position, text, color));
        }

        IEnumerator MakeTextCoroutine(Vector2Int position, string text, Color color)
        {
            string[] lines = text.Split('\n');

            foreach (var line in lines)
            {
                GameObject gameObject = Instantiate(instance.prefab);
                gameObject.GetComponentInChildren<TextMeshPro>().text = line;
                gameObject.GetComponentInChildren<TextMeshPro>().color = color;
                gameObject.transform.position = new Vector3(position.x, position.y);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}

