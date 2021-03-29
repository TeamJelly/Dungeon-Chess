using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace UI.Battle
{
    public class VisualEffectUI : MonoBehaviour
    {
        public static VisualEffectUI instance;

        Dictionary<string, RuntimeAnimatorController> visualEffects = new Dictionary<string, RuntimeAnimatorController>();

        [SerializeField]
        RuntimeAnimatorController[] animators;

        [SerializeField]
        GameObject prefab;

        private void Awake()
        {
            instance = this;

            animators = Resources.LoadAll<RuntimeAnimatorController>("VFXs");

            foreach (var animator in animators)
                visualEffects.Add(animator.name, animator);

            prefab = Resources.Load<GameObject>("VFXs/VFX_Default");
        }

        public static void MakeVisualEffect(Vector2Int position, string name)
        {
            GameObject gameObject = Instantiate(instance.prefab);
            gameObject.transform.position = new Vector3(position.x, position.y, -5);
            gameObject.GetComponent<Animator>().runtimeAnimatorController = instance.visualEffects[name];
        }
    }
}