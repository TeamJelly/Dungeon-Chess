using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Model.Managers;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public TextMeshProUGUI text;

        // Start is called before the first frame update

        public static HUD instance;
        private void Awake()
        {
            instance = this;
            text.text = $"골드: {GameManager.Instance.Gold}                  스테이지: {GameManager.Instance.stage}";
        }

        public static void SetText(string text)
        {
            instance.text.text = text;
        }

        public void EnableMenu()
        {
            PauseMenuUI.instance.Enable();
            //Debug.Log("Menu Enabled");
        }
    }
}