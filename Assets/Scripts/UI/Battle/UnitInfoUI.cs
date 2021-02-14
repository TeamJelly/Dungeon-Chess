using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Common.UI;
using Model;

namespace UI.Battle
{
    public class UnitInfoUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Name;
        [SerializeField]
        private Image Image;
        [SerializeField]
        private TextMeshProUGUI StatusText;
        [SerializeField]
        private Transform Move;
        [SerializeField]
        private Transform[] Items;
        [SerializeField]
        private Transform[] Skills;

        Sprite noSprite;

        Sprite NoSprite
        {
            get
            {
                if (noSprite == null)
                    noSprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
                return noSprite;
            }
        }

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
            Name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            Image = transform.Find("Image").GetComponent<Image>();
            StatusText = transform.Find("Status/ParameterText").GetComponent<TextMeshProUGUI>();
            Move = transform.Find("MovePanel/Move");
            Items = new Transform[2];
            Skills = new Transform[4];
            Items[0] = transform.Find("ItemPanel/item1");
            Items[1] = transform.Find("ItemPanel/item2");
            Skills[0] = transform.Find("SkillPanel/SkillIconPanel/Skill1");
            Skills[1] = transform.Find("SkillPanel/SkillIconPanel/Skill2");
            Skills[2] = transform.Find("SkillPanel/SkillIconPanel/Skill3");
            Skills[3] = transform.Find("SkillPanel/SkillIconPanel/Skill4");
        }

        private void Start()
        {
            SetUnitInfo(Model.Managers.GameManager.PartyUnits[0], true);
        }

        public void SetUnitInfo(Unit unit, bool interactable)
        {
            Name.text = unit.Name;
            Image.sprite = unit.Sprite;
            StatusText.text = $"{unit.UnitClass}\n" +
                $"{unit.Level}\n" +
                $"{unit.CurrentHP}/{unit.MaximumHP}\n" +
                $"{unit.CurrentEXP}/{unit.NextEXP}\n" +
                $"{unit.Strength}\n" +
                $"{unit.Agility}\n" +
                $"{unit.Move}";

            SetSkillSlot(Move, unit.MoveSkill, interactable);

            for (int i = 0; i < unit.Items.Length; i++)
                SetSkillSlot(Items[i], unit.Items[i], interactable);

            for (int i = 0; i < unit.Skills.Length; i++)
                SetSkillSlot(Skills[i], unit.Skills[i], interactable);
        }

        public void SetSkillSlot(Transform slot, Skill skill, bool interactable)
        {
            Button button = slot.GetComponent<Button>();
            Image image = slot.Find("Image").GetComponent<Image>();
            TextMeshProUGUI name = slot.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI count = slot.Find("Image/Count").GetComponent<TextMeshProUGUI>();

            if (skill == null)
            {
                button.interactable = false;
                image.sprite = NoSprite;
                name.text = "";
                count.text = "99";
                return;
            }
            else
            {
                button.interactable = interactable == true && skill.currentReuseTime == 0 ? true : false;
                image.sprite = skill.Sprite == null ? NoSprite : skill.Sprite;
                name.text = skill.enhancedLevel == 0 ? $"{skill.name}" : $"{skill.name}+{skill.enhancedLevel}";
                count.text = skill.currentReuseTime != 0 ? $"{skill.currentReuseTime}" : "";
            }
        }

        public void SetOtherUnitInfo(Unit unit)
        {

        }

    }
}