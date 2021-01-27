using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.UI;
using Model.Managers;
using Model.Models;

namespace UI
{
    public class HireUI : MonoBehaviour
    {
        public UnityEngine.UI.Image currentUnitImage;
        public Common.UI.Text startButton;
        Unit currentUnit;

        int selectedUnitCount = 0;
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
            UnitManager.Instance.AddPartyUnit(currentUnit);
            if (selectedUnitCount > 0) startButton.enabled = true;
        }
        public void Fire()
        {
            UnitManager.Instance.SubPartyUnit(currentUnit);
            if (selectedUnitCount == 0) startButton.enabled = false;
        }
    }
}