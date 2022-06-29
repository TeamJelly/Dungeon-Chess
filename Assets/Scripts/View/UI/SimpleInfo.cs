using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace View.UI
{
    public class SimpleInfo : MonoBehaviour
    {
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Type;
        public Image Image;
        public TextMeshProUGUI Description;

        public void Set(Infoable infoable)
        {
            Set(infoable.Name, infoable.Type, infoable.Sprite, infoable.Description);
        }

        public void Set(string name, string type, Sprite sprite, string description)
        {
            Name.text = name;
            Type.text = type;

            Image.sprite = sprite;
            // if (color != new Color(0,0,0,0))
            //     Image.color = color;
            // else
            Image.color = Color.white;

            Description.text = description;
        }
        private void OnValidate()
        {
            Name = transform.Find("Name/Text").GetComponent<TextMeshProUGUI>();
            Image = transform.Find("ImageMask/Image").GetComponent<Image>();
            Description = transform.Find("Description/Text").GetComponent<TextMeshProUGUI>();
        }

    }
}
