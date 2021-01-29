using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Common.UI;
using Model;

namespace UI.Battle
{
    public class UnitInfoUI : MonoBehaviour
    {
        public Image unitImage;

        public Text description;

        Unit allocatedUnit;
        public void Set(Unit unit)
        {
            allocatedUnit = unit;
            SetText(unit.name +
               "\nHP:" + unit.currentHP + "/" + unit.maxHP +
                "\nSPEED: " + unit.agility);

            unitImage.Sprite = unit.transform.GetComponent<SpriteRenderer>().sprite;
        }

        void SetText(string text)
        {
            //Debug.Log(description.Text);
            description.TextString = text;
        }

        public void Init()
        {
            gameObject.AddComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                UnitDescriptionUI.instance.Enable(allocatedUnit);
            });
        }
    }
}