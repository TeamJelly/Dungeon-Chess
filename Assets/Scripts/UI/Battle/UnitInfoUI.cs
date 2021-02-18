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
        private SkillSlotUI Move;
        [SerializeField]
        private SkillSlotUI[] Items;
        [SerializeField]
        private SkillSlotUI[] Skills;
        [SerializeField]
        private Button currentPushedButton;
        [SerializeField]
        private Unit unit;
        [SerializeField]
        private bool interactable;
        [SerializeField]
        private Sprite noSprite;
        [SerializeField]
        private TextMeshProUGUI EffectsText;

        [System.Serializable]
        public class SkillSlotUI
        {
            public Transform transform;
            public TextMeshProUGUI name;
            public TextMeshProUGUI count;
            public Image image;
            public Button button;

            public SkillSlotUI(Transform _transform)
            {
                this.transform = _transform;
                button = _transform.GetComponent<Button>();
                image = _transform.Find("Image").GetComponent<Image>();
                name = _transform.Find("Name").GetComponent<TextMeshProUGUI>();
                count = _transform.Find("Image/Count").GetComponent<TextMeshProUGUI>();
            }
        }

        Sprite NoSkill
        {
            get
            {
                if (noSprite == null)
                    noSprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/-");
                return noSprite;
            }
        }

        public Button CurrentPushedButton { get => currentPushedButton; set => currentPushedButton = value; }
        public Unit Unit { get => unit; set => unit = value; }

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
            Name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            Image = transform.Find("ImagePanel/Image").GetComponent<Image>();
            StatusText = transform.Find("Status/ParameterText").GetComponent<TextMeshProUGUI>();
            Move = new SkillSlotUI(transform.Find("MovePanel/Move"));
            Items = new SkillSlotUI[2];
            Skills = new SkillSlotUI[4];
            Items[0] = new SkillSlotUI(transform.Find("ItemPanel/item1"));
            Items[1] = new SkillSlotUI(transform.Find("ItemPanel/item2"));
            Skills[0] = new SkillSlotUI(transform.Find("SkillPanel/SkillIconPanel/Skill1"));
            Skills[1] = new SkillSlotUI(transform.Find("SkillPanel/SkillIconPanel/Skill2"));
            Skills[2] = new SkillSlotUI(transform.Find("SkillPanel/SkillIconPanel/Skill3"));
            Skills[3] = new SkillSlotUI(transform.Find("SkillPanel/SkillIconPanel/Skill4"));
            EffectsText = transform.Find("EffectsText").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {

        }

        public void SetUnitInfo(Unit unit, bool interactable)
        {
            this.unit = unit;
            this.interactable = interactable;

            UpdateUnitInfo();
        }

        public void UpdateUnitInfo()
        {
            Name.text = unit.Name;
            Image.sprite = unit.Sprite;
            StatusText.text = $"{unit.UnitClass}\n" +
                $"{unit.Level}\n" +
                $"{unit.CurrentHP} <color=#0000ff>+{unit.Armor}</color> /{unit.MaximumHP}\n" +
                $"{unit.CurrentEXP}/{unit.NextEXP}\n" +
                $"{unit.Strength}\n" +
                $"{unit.Agility}\n" +
                $"{unit.Move}";

            string EffectList = "";
            foreach (var effect in unit.StateEffects)
                EffectList += $"({effect.Name})";

            EffectsText.text = EffectList;

            SetSkillSlot(Move, unit.MoveSkill);

            for (int i = 0; i < unit.Items.Length; i++)
                SetSkillSlot(Items[i], unit.Items[i]);

            for (int i = 0; i < unit.Skills.Length; i++)
                SetSkillSlot(Skills[i], unit.Skills[i]);
        }

        public void SetSkillSlot(SkillSlotUI slot, Skill skill)
        {
            // 스킬이 없을 경우
            if (skill == null)
            {
                slot.button.interactable = false;
                slot.image.sprite = NoSkill;
                slot.name.text = "";
                slot.count.text = "";
                return;
            }
            // 스킬이 있다.
            slot.button.interactable = false;
            slot.image.sprite = skill.Sprite;
            slot.name.text = skill.enhancedLevel == 0 ? $"{skill.name}" : $"{skill.name} <color=#FF0000>+{skill.enhancedLevel}</color>";
            slot.count.text = skill.currentReuseTime == 0 ? "" : $"{skill.currentReuseTime}";

            // 아무 버튼도 안눌러져 있고, 이 버튼을 누를수 있는 경우
            if (currentPushedButton == null && skill.IsUsable(unit) && interactable == true)
            {
                slot.button.interactable = true;
                slot.button.onClick.RemoveAllListeners();
                slot.button.onClick.AddListener(() =>
                {
                    IndicatorUI.ShowTileIndicator(unit, skill);
                    currentPushedButton = slot.button;
                    UpdateUnitInfo();
                });
            }
            // 이미 이 버튼이 눌려있는 경우
            else if (currentPushedButton == slot.button)
            {
                slot.button.interactable = true;
                slot.button.onClick.RemoveAllListeners();
                slot.button.onClick.AddListener(() =>
                {
                    IndicatorUI.HideTileIndicator();
                    currentPushedButton = null;
                    UpdateUnitInfo();
                });
            }
        }
    }
}