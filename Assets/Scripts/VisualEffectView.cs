// using UnityEditor;
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// namespace View
// {
//     public class VisualEffectView : MonoBehaviour
//     {
//         public static VisualEffectView instance;

//         Dictionary<string, GameObject> VisualEffects = new Dictionary<string, GameObject>();

//         [SerializeField]
//         GameObject[] animators;

//         //[SerializeField]
//         //GameObject prefab;

//         private void Awake()
//         {
//             instance = this;

//             animators = Resources.LoadAll<GameObject>("VFXs");

//             foreach (var animator in animators)
//                 VisualEffects.Add(animator.name, animator);

//         }

//         public static void MakeVisualEffect(Vector2Int position, string name)
//         {
//             GameObject gameObject = Instantiate(instance.VisualEffects[name]);
//             gameObject.transform.position = new Vector3(position.x, position.y, -5);
//             // gameObject.GetComponent<Animator>().runtimeAnimatorController = instance.visualEffects[name];
//         }

//         public static void MakeDropEffect(Vector3 startPos, Vector3 target, Obtainable obtainable)
//         {
//             GameObject obj = BattleView.ObtainableObjects[obtainable];
//             obj.transform.position = startPos;

//             GameObject trailRenderObj = Instantiate(Resources.Load<GameObject>("TrailRenderObj"), obj.transform);
//             trailRenderObj.transform.localPosition = Vector3.zero;
//             IEnumerator Coroutine()
//             {
//                 float duration = 0.3f;
//                 float t = 0;

//                 Vector3 straightPosition = startPos;

//                 //Vector2 direction = new Vector2(target.x - startPos.x, target.y - startPos.y).normalized;
//                 //Vector2 curvedDiretion = new Vector2(direction.y, -direction.x);
//                 while (t < duration)
//                 {
//                     t += Time.deltaTime;
//                     straightPosition = Vector3.Lerp(startPos, target, t / duration);

//                     obj.transform.position = straightPosition; //+ (Vector3)curvedDiretion * Mathf.Sin(Mathf.PI * t / duration);
//                     //obj.transform.localScale = Vector3.one * (1 - Mathf.Sin(Mathf.PI * t / duration));
//                     yield return null;
//                 }
//                 obj.transform.position = target;
//                 Destroy(trailRenderObj);
//             }
//             instance.StartCoroutine(Coroutine());
//         }
//     }
// }