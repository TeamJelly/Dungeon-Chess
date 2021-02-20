using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;
using Model;
using Model.Units;

namespace UI
{
    public class HireUI : MonoBehaviour
    {

        public int maximumUnitCount = 0; // 인스펙터 창에서 세팅할 것.
        public int selectedUnitCount = 0;//현재 고용된 파티 인원 수
        [Header("[중앙 패널]")]
        public UnityEngine.UI.Image currentUnitImage;
        public Common.UI.Text selectButton;
        public UnityEngine.UI.Text descriptionText;

        [Header("[하단 패널]")]
        public Common.UI.UImage testImage;
        public List<Common.UI.UImage> partyImagies;
        public Common.UI.Text startButton;
        Unit currentUnit;
       
        int index = 0;

        List<Sprite> unitImagies = new List<Sprite>();
        private void Start()
        {
            List<Unit> units = UnitManager.Instance.AllUnits;

            for (int i = 0; i < units.Count; i++)
            {
                unitImagies.Add(units[i].Portrait);
            }
            UpdateUI();
        }
        void UpdateUI()
        {
            currentUnit = UnitManager.Instance.AllUnits[index];
            currentUnitImage.sprite = unitImagies[index];
            descriptionText.text = $"이름: {UnitManager.Instance.AllUnits[index].Name}\n";

            if (hasCurrentUnitInParty)
            {
                selectButton.TextString = "해고";
                selectButton.UsableButton = true;
            }
            else
            {
                selectButton.TextString = "고용";
                selectButton.UsableButton = selectedUnitCount != maximumUnitCount;
            }
            startButton.UsableButton = (selectedUnitCount > 0);

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
            if(index < unitImagies.Count - 1) index++;
            UpdateUI();
        }

        public void ShowPrevUnit()
        {
            if(index > 0) index--;
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

            for (int i = 0; i < maximumUnitCount; i++) partyImagies[i].Sprite = null;
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