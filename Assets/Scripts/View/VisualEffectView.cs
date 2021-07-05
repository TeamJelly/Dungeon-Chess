using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace View
{
    public class VisualEffectView : MonoBehaviour
    {
        public static VisualEffectView instance;

        Dictionary<string, GameObject> VisualEffects = new Dictionary<string, GameObject>();

        [SerializeField]
        GameObject[] animators;

        //[SerializeField]
        //GameObject prefab;

        private void Awake()
        {
            instance = this;

            animators = Resources.LoadAll<GameObject>("VFXs");

            foreach (var animator in animators)
                VisualEffects.Add(animator.name, animator);

            // prefab = Resources.Load<GameObject>("VFXs/VFX_Default");
        }

        public static void MakeVisualEffect(Vector2Int position, string name)
        {
            GameObject gameObject = Instantiate(instance.VisualEffects[name]);
            gameObject.transform.position = new Vector3(position.x, position.y, -5);
            // gameObject.GetComponent<Animator>().runtimeAnimatorController = instance.visualEffects[name];
        }
    }
}