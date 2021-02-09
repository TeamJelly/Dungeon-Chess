using Model;
using Model.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class GetSkillUI : MonoBehaviour
    {

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
        public Button setButton;

        Action<int, int> OnSlotClick;
        Skill skill;


        public static GetSkillUI instance;
        private void Awake()
        {
            instance = this;


        }
        public void Enable(string name)
        {
            gameObject.SetActive(true);

            //name으로 json 파일 파싱.

            //Skill 이미지, 설명 UI 갱신
            skill = new Skill_000();
            skillImage.sprite = skill.Sprite;
            skillDescription.text = $"{skill.name}\n{skill.description}";


            //UI 초기화
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
                    SkillSlots[i].transform.GetChild(j).GetComponent<Image>().sprite = GameManager.PartyUnits[i].Skills[j]?.Sprite;
                }
            }
            //패널 활성화
            SelectBehaviorPanel.SetActive(true);
            SelectSlotPanel.SetActive(false);
            ChangeSkillPanel.SetActive(false);
        }

        /******************************행동 선택창에서 쓰이는 함수**************************/
        public void EquipSkill()
        {
            OnSlotClick = EnableChangeSkillUI;
            SelectSlotPanel.SetActive(true);
        }

        public void UpgradeSkill()
        {
            OnSlotClick = EnableUpgradeSkillUI;
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
            beforeSkillImage.sprite = GameManager.PartyUnits[u].Skills[s]?.Sprite;
            beforeSkillText.text = GameManager.PartyUnits[u].Skills[s]?.GetDescription();

            afterSkillImage.sprite = skill.Sprite;
            afterSkillText.text = skill.GetDescription();

            setButton.onClick.RemoveAllListeners();
            setButton.onClick.AddListener(() =>
            {
                GameManager.PartyUnits[u].Skills[s] = skill;
                gameObject.SetActive(false);
            });
            ChangeSkillPanel.SetActive(true);
        }

        /// <summary>
        /// 스킬 강화 함수
        /// </summary>
        /// <param name="u">유닛 인덱스</param>
        /// <param name="s">슬롯 인덱스</param>
        void EnableUpgradeSkillUI(int u, int s)
        {
            if (GameManager.PartyUnits[u].Skills[s] == null) return;

            //등급 검사. 새 스킬 등급이 기존 스킬보다 작을 때 레전드 등급이 아닐경우에는 종료.
           /* if (skill.grade <= GameManager.PartyUnits[u].Skills[s].grade && skill.grade != Grade.Legend)
            {
                return;
            }*/

            beforeSkillImage.sprite = GameManager.PartyUnits[u].Skills[s].Sprite;
            beforeSkillText.text = GameManager.PartyUnits[u].Skills[s].GetDescription();

            //한단계 업그레이드 시의 설명을 보여주기 위해 임시로 레벨을 올렸다가 내림
            GameManager.PartyUnits[u].Skills[s].enhancedLevel++;
            afterSkillImage.sprite = skill.Sprite;
            afterSkillText.text = skill.GetDescription();
            GameManager.PartyUnits[u].Skills[s].enhancedLevel--;

            setButton.onClick.RemoveAllListeners();
            setButton.onClick.AddListener(() =>
            {
                GameManager.PartyUnits[u].Skills[s].enhancedLevel++;
                gameObject.SetActive(false);
            });

            ChangeSkillPanel.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }
        /***********************************************************************************/
    }
}