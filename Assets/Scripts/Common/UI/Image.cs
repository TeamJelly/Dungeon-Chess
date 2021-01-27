using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Common.UI
{
    public class Image : MonoBehaviour
    {
        private TMP_Text textMesh;

        [SerializeField]
        private UnityEngine.UI.Image image;
        private Button button;

        // inspector
        [SerializeField]
        private Data.Size size;
        [SerializeField]
        private bool isButton = true;
        [SerializeField]
        private bool usableButton = true;
        [SerializeField]
        private bool hasImage = true;
        [SerializeField]
        private bool hasText = true;
        [SerializeField]
        private Data.SizeField[] sizeDefinition =
        {
        new Data.SizeField { width = 80, height = 80 },
        new Data.SizeField { width = 180, height = 180 },
        new Data.SizeField { width = 600, height = 300 },
        new Data.SizeField { width = 300, height = 600 },
        new Data.SizeField { width = 1920, height = 1080 },
    };

        public Sprite Sprite
        {
            get => image.sprite;
            set => image.sprite = value;
        }
        public float ImageAlpha
        {
            get => image.color.a;
            set
            {
                if (value < 0f || value > 1f)
                {
                    Debug.LogError("value is not valid");
                }
                var color = image.color;
                color.a = value;
                image.color = color;
            }
        }

        public Data.Size Size
        {
            get => size;
            set
            {
                var v = (int)value;
                if (v < 0 || v >= sizeDefinition.Length)
                {
                    Debug.LogError("the size is not defined.");
                }
                size = value;
                image.rectTransform.sizeDelta = new Vector2(sizeDefinition[v].width, sizeDefinition[v].height);
                textMesh.rectTransform.offsetMin = new Vector2(textMesh.rectTransform.offsetMin.x, -sizeDefinition[v].height);
                textMesh.rectTransform.offsetMax = new Vector2(textMesh.rectTransform.offsetMax.x, -sizeDefinition[v].height);
            }
        }

        public float TextAlpha
        {
            get => textMesh.color.a;
            set
            {
                if (value < 0f || value > 1f)
                {
                    Debug.LogError("value is not valid");
                }
                var color = textMesh.color;
                color.a = value;
                textMesh.color = color;
            }
        }

        public bool IsButton
        {
            get => isButton;
            set
            {
                isButton = value;
                image.color = GetComponent<Button>().colors.normalColor;
                GetComponent<Button>().enabled = value;
            }
        }

        public bool UsableButton
        {
            get => usableButton;
            set
            {
                usableButton = value;
                button.interactable = value;
            }
        }

        private void OnValidate()
        {
            image = GetComponent<UnityEngine.UI.Image>();
            button = GetComponent<Button>();
            textMesh = transform.GetChild(0).GetComponent<TMP_Text>();
            Size = size;
            IsButton = isButton;
            UsableButton = usableButton;
            textMesh.gameObject.SetActive(hasText);
            ImageAlpha = hasImage ? 1f : 0f;
        }


        public void ChangeSize(Data.Size size)
        {
            Size = size;
        }

        public void ShowFrame()
        {
            hasImage = true;
        }

        public void HideFrame()
        {
            hasImage = false;
        }

        public void ShowText()
        {
            hasText = true;
        }

        public void HideText()
        {
            hasText = false;
        }

        public void EnableButton()
        {
            usableButton = true;
        }

        public void DisableButton()
        {
            usableButton = false;
        }

        public void Hello()
        {
            Debug.Log("Hello");
        }
    }
}
