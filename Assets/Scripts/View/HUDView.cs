using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Model.Managers;

namespace View
{
    public class HUDView : MonoBehaviour
    {
        public TextMeshProUGUI text;

        // Start is called before the first frame update
        public static HUDView instance;

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
            PauseMenuView.instance.Enable();
            //Debug.Log("Menu Enabled");
        }
    }
}