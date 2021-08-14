using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Model;

namespace View.UI
{
    public class UnitInfo : MonoBehaviour
    {
        public TextMeshProUGUI Name;
        public Image Portrait;
        public TextMeshProUGUI Status;

        public RectTransform Skills;
        public RectTransform Belongings;
        public RectTransform Effects;

        public GameObject infoBox;

        public void SetUnit(Unit unit)
        {
            Name.text = $"{unit.Modifier} {unit.Name}";
            Portrait.sprite = unit.Sprite;
            Status.text = $"{unit.Level}\n{unit.CurHP}/{unit.MaxHP}\n{unit.CurEXP}/{unit.NextEXP}\n{unit.Strength}\n{unit.Agility}\n{unit.Mobility}";

            Transform[] childList = Skills.transform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            Skills.sizeDelta = Vector2.zero;
            childList = Belongings.transform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            childList = Effects.transform.GetComponentsInChildren<Transform>();
            Belongings.sizeDelta = Vector2.zero;
            for (int i = 1; i < childList.Length; i++)
                Destroy(childList[i].gameObject);
            Effects.sizeDelta = Vector2.zero;

            foreach (Skill skill in unit.Skills.Values)
            {
                GameObject gameObject = Instantiate(infoBox, Skills);

                Image image = gameObject.transform.Find("Image").GetComponent<Image>();
                image.sprite = skill.Sprite;
                if (image.color != new Color(0, 0, 0, 0))
                    image.color = skill.Color;

                Rect rect = gameObject.GetComponent<RectTransform>().rect;
                Skills.sizeDelta = Skills.rect.size + new Vector2(rect.width, 0);
                gameObject.transform.SetParent(Skills);

                Button button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => InfoView.Show(unit, skill));
            }

            foreach (Obtainable obtainable in unit.Belongings)
            {
                GameObject gameObject = Instantiate(infoBox, Belongings);

                Image image = gameObject.transform.Find("Image").GetComponent<Image>();
                image.sprite = obtainable.Sprite;

                Rect rect = gameObject.GetComponent<RectTransform>().rect;
                Belongings.sizeDelta = Belongings.rect.size + new Vector2(rect.width, 0);
                gameObject.transform.SetParent(Belongings);

                Button button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => InfoView.Show(obtainable));
            }

            foreach (Effect effect in unit.StateEffects)
            {
                GameObject gameObject = Instantiate(infoBox, Effects);

                Image image = gameObject.transform.Find("Image").GetComponent<Image>();
                image.sprite = effect.Sprite;

                Rect rect = gameObject.GetComponent<RectTransform>().rect;
                Effects.sizeDelta = Effects.rect.size + new Vector2(rect.width, 0);
                gameObject.transform.SetParent(Effects);

                Button button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => InfoView.Show(effect));
            }
        }

        private void OnValidate()
        {
            Name = transform.Find("Name/Text").GetComponent<TextMeshProUGUI>();
            Status = transform.Find("Status/Parameter").GetComponent<TextMeshProUGUI>();
            Portrait = transform.Find("Portrait/Image").GetComponent<Image>();

            Skills = transform.Find("Skills/Viewport/Content").GetComponent<RectTransform>();
            Skills.sizeDelta = Vector2.zero;

            Belongings = transform.Find("Belongings/Viewport/Content").GetComponent<RectTransform>();
            Belongings.sizeDelta = Vector2.zero;

            Effects = transform.Find("Effects/Viewport/Content").GetComponent<RectTransform>();
            Effects.sizeDelta = Vector2.zero;

            infoBox = Resources.Load<GameObject>("Prefabs/UI/InfoBox");
        }

    }

}
