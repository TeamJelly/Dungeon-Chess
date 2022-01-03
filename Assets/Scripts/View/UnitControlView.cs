using Model;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.UI;

namespace View
{
    public class UnitControlView : MonoBehaviour
    {
        public GameObject panel;
        [Space]

        public GameObject skillPanel;
        public SkillButton[] skillButtons;
        private int skillCount = 4;
        [Space]

        public GameObject itemPanel;
        public ItemButton[] itemButtons;
        private int itemCount = 3;

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
                SkillButton newButton = skillPanel.transform.GetChild(i).GetComponent<SkillButton>();
                newButton.Init();
                skillButtons[i] = newButton;
            }

            itemButtons = new ItemButton[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                ItemButton newButton = itemPanel.transform.GetChild(i).GetComponent<ItemButton>();
                newButton.Init();
                itemButtons[i] = newButton;
            }
            UpdateItemButtons();
        }

        //SkillCount와 MoveCount의 구분이 불분명함.
        //Move도 일종의 skill이라면 SkillCount의 영향을 받아야 되는것이 아닌가 생각
        public void UpdateSkillButtons(Unit unit)
        {
            //스킬(아이템, 이동 포함) 이미지 갱신
            //버튼 이벤트 등록
            for (int i = 0; i < 4; i++)
                skillButtons[i].SetSkill(null, null);

            skillButtons[0].SetSkill(unit, unit.MoveSkill); 

            for (int i = 0; i < 3 && i < unit.Skills.Count; i++)
                skillButtons[i+1].SetSkill(unit, unit.Skills[i]);
          
            RefreshButtons();
        }

        public void UpdateItemButtons()
        {

            for(int i = 0; i < GameManager.Instance.itemBag.Count; i++)
                itemButtons[i].SetItem(GameManager.Instance.itemBag[i]);

            for(int i = GameManager.Instance.itemBag.Count; i< 3; i++)
                itemButtons[i].SetItem(null);

            RefreshButtons();
        }

        public void RefreshButtons()
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

            for (int i = 0; i < itemCount; i++)
            {
                ItemButton button = itemButtons[i];
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

            foreach (var itemButton in itemButtons)
                itemButton.SetInteractable(!itemButton.properties.interactable);
        }
    }
}