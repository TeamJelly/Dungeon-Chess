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
            GetComponentInChildren<SpriteRenderer>().color = HPBar.AllianceToColorDict[Model.Managers.BattleManager.instance.thisTurnUnit.Alliance];
            transform.position = (Vector3Int)(Model.Managers.BattleManager.instance.thisTurnUnit.Position + Vector2Int.up);
        }
    }
}



