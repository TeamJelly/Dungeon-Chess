using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace View.UI
{
    public class TurnCursor : MonoBehaviour
    {
        void FixedUpdate()
        {
            if (Model.Managers.BattleManager.instance.thisTurnUnit != null)
            {
                GetComponentInChildren<SpriteRenderer>().color = HPBar.AllianceToColorDict[Model.Managers.BattleManager.instance.thisTurnUnit.Alliance];
                transform.position = (Vector3Int)(Model.Managers.BattleManager.instance.thisTurnUnit.Position) + Vector3.up + Vector3.back;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().color = new Color(0,0,0,0);
            }
        }
    }
}



