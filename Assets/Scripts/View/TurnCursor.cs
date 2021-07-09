using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace UI.Battle
{
    public class TurnCursor : MonoBehaviour
    {
        void FixedUpdate()
        {
            if (Model.Managers.BattleManager.instance.thisTurnUnit.Alliance == Model.UnitAlliance.Party)
                GetComponentInChildren<Image>().color = Color.green;
            else if (Model.Managers.BattleManager.instance.thisTurnUnit.Alliance == Model.UnitAlliance.Enemy)
                GetComponentInChildren<Image>().color = Color.red;

            transform.position = 
                Camera.main.WorldToScreenPoint(
                    BattleView.UnitObjects[Model.Managers.BattleManager.instance.thisTurnUnit].transform.position
                    + Vector3.up);
        }
    }

}

