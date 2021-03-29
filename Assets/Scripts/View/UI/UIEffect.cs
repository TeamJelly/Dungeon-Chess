using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UI.Battle;
using UnityEngine;

namespace Common.UI
{
    public class UIEffect : MonoBehaviour
    {
        public static void FadeInPanel(GameObject panel)
        {
            CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            panel.SetActive(true);
            panel.GetComponent<MonoBehaviour>().StartCoroutine(FadeInCoroutine(panel,canvasGroup));
        }

        public static void FadeOutPanel(GameObject panel)
        {
            CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            panel.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCoroutine(panel, canvasGroup));
        }
        static IEnumerator FadeInCoroutine(GameObject panel, CanvasGroup canvasGroup)
        {
            float time = 0.1f;
            float value = 0;
            canvasGroup.alpha = 0;
            panel.SetActive(true);
            while (value < time)
            {
                value += Time.deltaTime;

                canvasGroup.alpha = value / time;
                yield return null;
            }
            canvasGroup.alpha = 1;
        }
        static IEnumerator FadeOutCoroutine(GameObject panel, CanvasGroup canvasGroup)
        {
            float time = 0.1f;
            float value = time;
            canvasGroup.alpha = 1;
            while (value > 0)
            {
                value -= Time.deltaTime;

                canvasGroup.alpha = value / time;
                yield return null;
            }
            canvasGroup.alpha = 0;
            panel.SetActive(false);
        }
    }
}