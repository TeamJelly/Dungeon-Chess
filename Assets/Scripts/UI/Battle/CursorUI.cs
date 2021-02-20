using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Battle
{
    public class CursorUI : MonoBehaviour
    {
        void FixedUpdate()
        {
            transform.position = 
                Camera.main.WorldToScreenPoint(
                    BattleUI.instance.UnitObjects[Model.Managers.BattleManager.instance.thisTurnUnit].transform.position
                    + Vector3.up);
        }
    }

}

