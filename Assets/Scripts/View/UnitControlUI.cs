using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Battle
{
    public class UnitControlUI : MonoBehaviour
    {
        public GameObject panel;
        //나중에 private로 바꾸기
        public SkillButton[] skillButtons;
        private int skillCount = 6;

        // Start is called before the first frame update

        private void Awake()
        {
            InitUI();
        }

        void InitUI()
        {
            skillButtons = new SkillButton[skillCount];
            for (int i = 0; i < skillCount; i++)
            {
                skillButtons[i] = panel.transform.GetChild(i).GetComponent<SkillButton>();
                skillButtons[i].toggleOption = true;
            }
        }

        //SkillCount와 MoveCount의 구분이 불분명함.
        //Move도 일종의 skill이라면 SkillCount의 영향을 받아야 되는것이 아닌가 생각
        public void UpdateUI(Unit unit)
        {
            //스킬(아이템, 이동 포함) 이미지 갱신
            //버튼 이벤트 등록

            for (int i = 0; i < skillCount; i++)
            {
                SkillButton button = skillButtons[i];
                //button.MainImage.sprite = unit.Skills[i].Sprite;
                button.properties.onClick.RemoveAllListeners();


                button.properties.onClick.AddListener(() =>
                {
                    for (int j = 0; j < skillCount; j++)
                    {
                        skillButtons[j].SetInteractable(!button.pressed);
                    }
                    button.SetInteractable(true);

                });
            }
            skillButtons[0].SetSkill(unit, unit.MoveSkill);
            skillButtons[1].SetSkill(unit, unit.PassiveSkill);
            for (int i = 0; i < 4; i++)
            {
                SkillButton button = skillButtons[i + 2];
                button.SetSkill(unit, unit.Skills[i]);
            }
            RefreshButtons(unit);
        }

        public void RefreshButtons(Unit unit)
        {
            for (int i = 0; i < skillCount; i++)
            {
                SkillButton button = skillButtons[i];
                if (button.pressed)
                {
                    button.ShowPopEffect();
                    button.pressed = false;
                }
                button.SetInteractable(true);
            }
        }
    }
}