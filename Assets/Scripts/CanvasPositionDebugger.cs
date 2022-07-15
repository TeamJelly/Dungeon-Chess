using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class CanvasPositionDebugger : MonoBehaviour
    {
        public Button button;

        void Awake()
        {
            button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(DebugPosition);
        }

        void DebugPosition()
        {
            Debug.Log("Position: " + transform.position);
            Debug.Log("Rect transform Position: " + ((RectTransform)transform).position);
            Debug.Log("Anchored Position: " + ((RectTransform)transform).anchoredPosition);
            Debug.Log("SizeDelta: " + ((RectTransform)transform).sizeDelta);
        }
    }

}
