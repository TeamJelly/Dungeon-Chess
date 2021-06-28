using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;
using Model;
using Model.Units;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class HireUI : MonoBehaviour
    {
        public int maximumUnitCount = 0; // 인스펙터 창에서 세팅할 것.
        public int selectedUnitCount = 0;//현재 고용된 파티 인원 수
        [Header("[중앙 패널]")]

        public Button nextButton;
        public Button prevButton;
        public Image currentUnitImage;
        public Button hireButton;
        public TextMeshProUGUI descriptionText;

        [Header("[하단 패널]")]
        public Common.UI.UImage testImage;
        public List<Common.UI.UImage> partyImagies;
        public Button startButton;
        Unit currentUnit;
       
        public int index = 0;

        List<Sprite> unitImagies = new List<Sprite>();

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
        private void Start()
        {
            // HUD.SetText("유닛 선택");
            hireButton.onClick.AddListener(SelectUnit);
            prevButton.interactable = false;
            List<Unit> units = UnitManager.Instance.AllUnits;

            for (int i = 0; i < units.Count; i++)
            {
                unitImagies.Add(units[i].Portrait);
            }
            nextButton.onClick.AddListener(ShowNextUnit);
            prevButton.onClick.AddListener(ShowPrevUnit);
            for (int i = 0; i < maximumUnitCount; i++) partyImagies[i].Sprite = NoSkill;
            UpdateUI();
        }
        void UpdateUI()
        {
            currentUnit = UnitManager.Instance.AllUnits[index];
            currentUnitImage.sprite = unitImagies[index];
            descriptionText.text =   $"{currentUnit.UnitClass}\n" +
                                $"{currentUnit.Level}\n" +
                                $"{currentUnit.CurrentHP} <color=#0000ff>+{currentUnit.Armor}</color> /{currentUnit.MaximumHP}\n" +
                                $"{currentUnit.CurrentEXP}/{currentUnit.NextEXP}\n" +
                                $"{currentUnit.Strength}\n" +
                                $"{currentUnit.Agility}\n" +
                                $"{currentUnit.Move}";

            if (hasCurrentUnitInParty)
            {
                hireButton.GetComponentInChildren<TextMeshProUGUI>().text = "해고";
                hireButton.enabled = true;
            }
            else
            {
                hireButton.GetComponentInChildren<TextMeshProUGUI>().text = "고용";
                hireButton.enabled = selectedUnitCount != maximumUnitCount;
            }
            startButton.enabled = (selectedUnitCount > 0);

        }
        public void EnableStartButton()
        {

        }

        public void DIsableStartButton()
        {

        }

        //UnitManager에서 Unit을 가져와서 화면에 보여주기.
        public void ShowNextUnit()
        {
            
            if (index < unitImagies.Count - 1)
            {
                prevButton.interactable = true;
                index++;
            }
            if(index == unitImagies.Count - 1) nextButton.interactable = false;
            UpdateUI();
        }

        public void ShowPrevUnit()
        {
            
            if (index > 0)
            {
                nextButton.interactable = true;
                index--;
            }
            if(index == 0) prevButton.interactable = false;
            UpdateUI();
        }

        public void SelectUnit()
        {
            if(hasCurrentUnitInParty)
            {
                Fire();
            }
            else
            {
                Hire();
            }

            for (int i = 0; i < maximumUnitCount; i++) partyImagies[i].Sprite = NoSkill;
            for (int i = 0; i < GameManager.PartyUnits.Count; i++)
            {
                int index = UnitManager.Instance.AllUnits.IndexOf(GameManager.PartyUnits[i]);
                partyImagies[i].Sprite = unitImagies[index];
            }
            UpdateUI();
        }
        void Hire()
        {
            GameManager.AddPartyUnit(currentUnit);
            selectedUnitCount++;
        }
        void Fire()
        {
            GameManager.RemovePartyUnit(currentUnit);
            selectedUnitCount--;
            if (selectedUnitCount == 0) startButton.enabled = false;
        }
        private bool hasCurrentUnitInParty
        {
            get
            {
                return GameManager.PartyUnits.Contains(currentUnit);
            }
        }
    }
}