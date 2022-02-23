// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using Model;
// using System.Linq;
// using DG.Tweening;

// namespace View
// {
//     public class FadeOutTextView : MonoBehaviour
//     {
//         public static FadeOutTextView instance;
//         public GameObject prefab;

//         // public bool isCoroutineRunning = false;

//         public class FadeOutText
//         {
//             public Unit unit;
//             public string text;
//             public Color color;
//         }

//         public Dictionary<Unit, Queue<FadeOutText>> WaitingQueue = new Dictionary<Unit, Queue<FadeOutText>>();

//         private void Awake()
//         {
//             instance = this;
//             prefab = Resources.Load<GameObject>("Prefabs/UI/FadeOutText");
//         }

//         public static void MakeText(FadeOutText fadeOutText)
//         {
//             if (!instance.WaitingQueue.ContainsKey(fadeOutText.unit))
//                 instance.WaitingQueue.Add(fadeOutText.unit, new Queue<FadeOutText>());

//             instance.WaitingQueue[fadeOutText.unit].Enqueue(fadeOutText);
//             // PlayText();
//         }

//         public static void MakeText(Unit unit, string text, Color color)
//         {
//             MakeText(new FadeOutText() { unit = unit, text = text, color = color });
//         }
//         public static Sequence MakeTextSequence(FadeOutText fadeOutTexts)
//         {
//             Sequence sequence = DOTween.Sequence();

//             GameObject gameObject = Instantiate(instance.prefab);

//             return sequence;
//         }

//         public static Sequence MakeTextSequence(List<FadeOutText> fadeOutTexts)
//         {
//             Sequence sequence = DOTween.Sequence();

//             return sequence;
//         }

//         // public static void PlayText()
//         // {
//         //     if (instance.isCoroutineRunning == false)
//         //         instance.StartCoroutine(instance.TextCoroutine());
//         // }

//         // public static void StopText()
//         // {
//         //     if (instance.isCoroutineRunning == true)
//         //         instance.StopCoroutine(instance.TextCoroutine());
//         // }

//         // IEnumerator TextCoroutine()
//         // {
//         //     isCoroutineRunning = true;

//         //     while (WaitingQueue.Count > 0)
//         //     {
//         //         foreach (var item in WaitingQueue.Values.ToArray())
//         //         {
//         //             FadeOutText line = item.Dequeue();
//         //             GameObject gameObject = Instantiate(instance.prefab);
//         //             gameObject.GetComponentInChildren<TextMeshPro>().text = line.text;
//         //             gameObject.GetComponentInChildren<TextMeshPro>().color = line.color;
//         //             gameObject.transform.position = new Vector3(line.unit.Position.x, line.unit.Position.y + 1);

//         //             if (item.Count == 0)
//         //                 WaitingQueue.Remove(line.unit);
//         //         }

//         //         yield return new WaitForSeconds(Model.Managers.GameManager.AnimationDelaySpeed);
//         //     }

//         //     isCoroutineRunning = false;
//         // }
//     }
// }