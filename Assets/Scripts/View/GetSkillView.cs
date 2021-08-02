using Model;
using Model.Skills;
using Model.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Runtime.CompilerServices;

namespace View
{
    public class GetSkillView : MonoBehaviour
    {
        public GameObject panel;

        [Header("행동 선택창")]
        public GameObject SelectBehaviorPanel;
        public Image skillImage;
        public TextMeshProUGUI skillDescription;

        [Header("슬롯 선택창")]
        public GameObject SelectSlotPanel;
        public Image[] unitImagies;
        public GameObject[] SkillSlots;

        [Header("스킬 교체창")]
        public GameObject ChangeSkillPanel;
        
        public Image beforeSkillImage;
        public Image afterSkillImage;
        public TextMeshProUGUI beforeSkillText;
        public TextMeshProUGUI afterSkillText;

        public TextMeshProUGUI beforeSkillTitle;
        public TextMeshProUGUI afterSkillTitle;
        public Button setButton;
        public Button cancelButton;

        Action<int, int> OnSlotClick;
        private Skill skill;

        private Sprite noSprite;
        Sprite NoSkill
        {
            get
            {
                if (noSprite == null)
                    noSprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/-");
                return noSprite;
            }
        }
        public static GetSkillView instance;
        private void Awake()
        {
            instance = this;


        }

        public void Enable(Skill newSkill)
        {
            UIEffect.FadeInPanel(panel);
            Debug.Log(newSkill);
            skill = newSkill;
            //Skill 이미지, 설명 UI 갱신
            skillImage.sprite = skill.Sprite;
            skillDescription.text = $"<{skill.Name}>\n\n{skill.Description}";

            //UI 초기화
            for (int i = 0; i < 4; i++)
                unitImagies[i].transform.parent.gameObject.SetActive(false);

            for (int i = 0; i < GameManager.PartyUnits.Count; i++)
            {
                //유닛 초상화 세팅
                unitImagies[i].sprite = GameManager.PartyUnits[i].Sprite;
                int unitIndex = i;

                //유닛마다의 스킬 패널 세팅
                for (int j = 0; j < SkillSlots.Length; j++)
                {
                    int skillIndex = j;
                    SkillSlots[i].transform.GetChild(j).GetComponent<Button>().onClick.AddListener(() => OnSlotClick(unitIndex, skillIndex));
                    SkillSlots[i].transform.GetChild(j).GetComponent<Image>().sprite = GameManager.PartyUnits[i].Skills[j] == null ? NoSkill : GameManager.PartyUnits[i].Skills[j].Sprite;
                }
            }

            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() =>
            {
                UIEffect.FadeOutPanel(ChangeSkillPanel);
            });

            //패널 활성화
            SelectBehaviorPanel.SetActive(true);
            SelectSlotPanel.SetActive(false);
            ChangeSkillPanel.SetActive(false);
        }
        public void Enable(string name)
        {
            skill = (Skill)Activator.CreateInstance(Type.GetType($"Model.Skills.{name}"));
            Enable(skill);

            
        }

        /******************************행동 선택창에서 쓰이는 함수**************************/
        public void EquipSkill()
        {
            OnSlotClick = EnableChangeSkillUI;
            for (int i = 0; i < GameManager.PartyUnits.Count; i++)
            {
                bool isActive = true;
                for (int j = 0; j < GameManager.PartyUnits[i].Skills.Length; j++)
                    if (GameManager.PartyUnits[i].Skills[j]?.Name == skill.Name)
                    {
                        isActive = false;
                        break;
                    }

                // isActive = isActive && skill.UnitClass == GameManager.PartyUnits[i].Class;
                unitImagies[i].transform.parent.gameObject.SetActive(isActive);

            }

            SelectSlotPanel.SetActive(true);
        }

        public void UpgradeSkill()
        {
            OnSlotClick = EnableUpgradeSkillUI;

            for (int i = 0; i < GameManager.PartyUnits.Count; i++)
                unitImagies[i].transform.parent.gameObject.SetActive(true);
            SelectSlotPanel.SetActive(true);
        }

        /******************************스킬 교체창에서 쓰이는 함수**************************/

        /// <summary>
        /// 스킬 교체함수
        /// </summary>
        /// <param name="u">유닛 인덱스</param>
        /// <param name="s">슬롯 인덱스</param>
        void EnableChangeSkillUI(int u, int s)
        {
            beforeSkillTitle.text = GameManager.PartyUnits[u].Skills[s]?.Level != 0 ? GameManager.PartyUnits[u].Skills[s]?.Name + $" +{GameManager.PartyUnits[u].Skills[s]?.Level}"
                                                                                    : GameManager.PartyUnits[u]?.Skills[s].Name;
            beforeSkillImage.sprite = GameManager.PartyUnits[u].Skills[s]?.Sprite;
            beforeSkillText.text = GameManager.PartyUnits[u].Skills[s]?.GetDescription(GameManager.PartyUnits[u]);

            afterSkillTitle.text = skill.Level != 0 ? skill.Name + $" +{skill.Level}" : skill.Name;
            afterSkillImage.sprite = skill.Sprite;
            afterSkillText.text = skill.GetDescription(GameManager.PartyUnits[u]);

            setButton.onClick.RemoveAllListeners();
            setButton.onClick.AddListener(() =>
            {
                Common.Command.AddSkill(GameManager.PartyUnits[u], skill, s);
                //.Skills[s] = skill;
                Disable();
            });
            UIEffect.FadeInPanel(ChangeSkillPanel);
        }

        /// <summary>
        /// 스킬 강화 함수
        /// </summary>
        /// <param name="u">유닛 인덱스</param>
        /// <param name="s">슬롯 인덱스</param>
        void EnableUpgradeSkillUI(int u, int s)
        {
            if (GameManager.PartyUnits[u].Skills[s] == null) return;
            if (GameManager.PartyUnits[u].Skills[s].Level == GameManager.PartyUnits[u].Skills[s].MaxLevel) return;
            //등급 검사. 새 스킬 등급이 기존 스킬보다 작을 때 레전드 등급이 아닐경우에는 종료.
            /*if (skill.Grade <= GameManager.PartyUnits[u].Skills[s].Grade && skill.Grade != Grade.Legend)
            {
                return;
            }*/

            beforeSkillTitle.text = GameManager.PartyUnits[u].Skills[s].Level != 0 ? GameManager.PartyUnits[u].Skills[s].Name + $" +{GameManager.PartyUnits[u].Skills[s].Level}"
                                                                                    :GameManager.PartyUnits[u].Skills[s].Name;
            beforeSkillImage.sprite = GameManager.PartyUnits[u].Skills[s].Sprite;
            beforeSkillText.text = GameManager.PartyUnits[u].Skills[s].GetDescription(GameManager.PartyUnits[u]);

            //한단계 업그레이드 시의 설명
            afterSkillTitle.text = GameManager.PartyUnits[u].Skills[s].Name + $" +{GameManager.PartyUnits[u].Skills[s].Level + 1}";
            afterSkillImage.sprite = GameManager.PartyUnits[u].Skills[s].Sprite;
            afterSkillText.text = GameManager.PartyUnits[u].Skills[s].GetDescription(GameManager.PartyUnits[u], GameManager.PartyUnits[u].Skills[s].Level + 1);

            setButton.onClick.RemoveAllListeners();
            setButton.onClick.AddListener(() =>
            {
                Common.Command.UpgradeSkill(GameManager.PartyUnits[u], s);
                Disable();
            });
            UIEffect.FadeInPanel(ChangeSkillPanel);
        }
        public void Disable()
        {
            UIEffect.FadeOutPanel(panel);
        }
        /***********************************************************************************/
    }
}