using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Managers
{
    public class EventRoomManager : MonoBehaviour
    {
        public void HealHP(int value)
        {
            foreach (Unit unit in GameManager.PartyUnits)
            {
                if (unit.CurrentHP + value > unit.MaximumHP)
                    value = unit.MaximumHP - unit.CurrentHP;

                unit.CurrentHP += value;
            }

            MenuManager.instance.GotoStage();

        }

        public void UpgradeSkill()
        {
            foreach (Unit unit in GameManager.PartyUnits)
            {
                UnitAction.EnhanceSkill(unit, 0);
            }

            MenuManager.instance.GotoStage();
        }
    }
}