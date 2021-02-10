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
        public UImage unitImage;

        public Text description;

        Unit allocatedUnit;
        public void Set(Unit unit)
        {
            allocatedUnit = unit;

            string effectNames = "";
            foreach (var effect in unit.StateEffects)
            {
                effectNames += $"({effect.name}) ";
            }

            SetText(
                $"{unit.Name}\n" +
                $"{unit.Level}\n" +
                $"{unit.CurrentHP}/{unit.MaximumHP}\n" +
                $"{unit.CurrentEXP}/{unit.NextEXP}\n" +
                $"{unit.Strength}\n" +
                $"{unit.Agility}\n" +
                $"{unit.Move}\n" +
                $"{effectNames}"
                );
            unitImage.Sprite = unit.Sprite;
        }

        void SetText(string text)
        {
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