using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.UI;

namespace View
{
    public class UnitControlView : MonoBehaviour
    {
        public GameObject panel;
        public SkillButton[] skillButtons;
        private int skillCount = 4;

        public static UnitControlView instance;

        // Start is called before the first frame update
        private void Awake()
        {
            instance = this;
            InitUI();
        }

        void InitUI()
        {
            skillButtons = new SkillButton[skillCount];
            for (int i = 0; i < skillCount; i++)
            {
                SkillButton newButton = panel.transform.GetChild(i).GetComponent<SkillButton>();
                newButton.Init();
                skillButtons[i] = newButton;
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
            }

            skillButtons[0].SetSkill(unit, unit.Skills[SkillCategory.Move]);
            skillButtons[1].SetSkill(unit, unit.Skills[SkillCategory.Basic]);
            skillButtons[2].SetSkill(unit, unit.Skills[SkillCategory.Intermediate]);
            skillButtons[3].SetSkill(unit, unit.Skills[SkillCategory.Advanced]);

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

        public void ToggleAllButtons()
        {
            foreach (var skillButton in skillButtons)
                skillButton.SetInteractable(!skillButton.properties.interactable);
        }
    }
}