using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Model.Managers;
using UI.Popup;

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
            UpdateUI();
        }

        public static void SetText(string text)
        {
            instance.text.text = text;
        }

        public void UpdateUI()
        {
            text.text = $"골드: {GameManager.Instance.Gold}                  스테이지: {GameManager.Instance.stage}";
        }

        public void EnableMenu()
        {
            PauseMenuUI.instance.Enable();
            //Debug.Log("Menu Enabled");
        }
    }
}