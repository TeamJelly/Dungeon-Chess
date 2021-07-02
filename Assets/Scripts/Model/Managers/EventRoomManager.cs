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
                if (unit.CurHP + value > unit.MaxHP)
                    value = unit.MaxHP - unit.CurHP;

                unit.CurHP += value;
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