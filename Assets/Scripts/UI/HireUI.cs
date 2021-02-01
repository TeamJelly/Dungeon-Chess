using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.UI;
using Model.Managers;
using Model;

namespace UI
{
    public class HireUI : MonoBehaviour
    {
        public UnityEngine.UI.Image currentUnitImage;
        public Common.UI.Text startButton;
        Unit currentUnit;

        int selectedUnitCount = 0;//현재 고용된 파티 인원
        int index = 0;

        private void Start()
        {
            
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

        }

        public void ShowPrevUnit()
        {

        }

        public void Hire()
        {
            GameManager.AddPartyUnit(currentUnit);
            if (selectedUnitCount > 0) startButton.enabled = true;
        }
        public void Fire()
        {
            GameManager.RemovePartyUnit(currentUnit);
            if (selectedUnitCount == 0) startButton.enabled = false;
        }
    }
}