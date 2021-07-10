using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.Managers;
using Model;
using UnityEngine.UI;
using TMPro;

namespace View.UI
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
        // public UImage testImage;
        public List<Image> partyImagies;
        public Button startButton;
        public Image crownImage;
        Unit currentUnit;
        int index = 0;
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
            startButton.onClick.AddListener(MenuManager.instance.GotoLobby);
            prevButton.interactable = false;
            List<Unit> units = UnitManager.Instance.AllUnits;

            for (int i = 0; i < units.Count; i++)
            {
                unitImagies.Add(units[i].Portrait);
            }
            nextButton.onClick.AddListener(ShowNextUnit);
            prevButton.onClick.AddListener(ShowPrevUnit);
            for (int i = 0; i < maximumUnitCount; i++)
            {
                int idx = i;
                partyImagies[i].gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (selectedUnitCount <= idx) return;
                    GameManager.LeaderUnit = GameManager.PartyUnits[idx];
                    SetCrownPosition(idx);
                    ShowUnit(idx);
                });
                partyImagies[i].sprite = NoSkill;
            }

            UpdateUI();
        }

        void SetCrownPosition(int idx)
        {
            crownImage.transform.SetParent(partyImagies[idx].transform);
            ((RectTransform)crownImage.transform).anchoredPosition = Vector2.zero;
            crownImage.gameObject.SetActive(true);
        }
        void UpdateUI()
        {
            currentUnit = UnitManager.Instance.AllUnits[index];
            currentUnitImage.sprite = unitImagies[index];
            descriptionText.text =   
                                $"{currentUnit.Class}\n" +
                                $"{currentUnit.Level}\n" +
                                $"{currentUnit.CurHP} <color=#0000ff>+{currentUnit.Armor}</color> /{currentUnit.MaxHP}\n" +
                                $"{currentUnit.CurEXP}/{currentUnit.NextEXP}\n" +
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
            crownImage.gameObject.SetActive(selectedUnitCount > 0);
        }
        public void EnableStartButton()
        {

        }

        public void DIsableStartButton()
        {

        }

        //하단 고용된 유닛 이미지 클릭 시 해당 유닛 정보를 보여줌.
        void ShowUnit(int idx)
        {
            nextButton.interactable = idx < unitImagies.Count - 1;
            prevButton.interactable = idx > 0;
            index = idx;
            UpdateUI();
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

            //리더가 없는 상태면 첫번째 유닛을 리더로 등록.
            if (GameManager.LeaderUnit == null && selectedUnitCount > 0)
            {
                GameManager.LeaderUnit = GameManager.PartyUnits[0];
            }
            for (int i = 0; i < maximumUnitCount; i++) partyImagies[i].sprite = NoSkill;
            for (int i = 0; i < GameManager.PartyUnits.Count; i++)
            {
                int index = UnitManager.Instance.AllUnits.IndexOf(GameManager.PartyUnits[i]);
                partyImagies[i].sprite = unitImagies[index];
                if (GameManager.PartyUnits[i] == GameManager.LeaderUnit) SetCrownPosition(i);
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
            if (GameManager.LeaderUnit == currentUnit) // 해고하려는 유닛이 리더인 경우.
            {
                GameManager.LeaderUnit = null;
            }
            
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