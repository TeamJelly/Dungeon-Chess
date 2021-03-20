using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class CursorUI : MonoBehaviour
    {
        void FixedUpdate()
        {
            if (Model.Managers.BattleManager.instance.thisTurnUnit.Category == Category.Party)
                GetComponentInChildren<Image>().color = Color.green;
            else if (Model.Managers.BattleManager.instance.thisTurnUnit.Category == Category.Enemy)
                GetComponentInChildren<Image>().color = Color.red;

            transform.position = 
                Camera.main.WorldToScreenPoint(
                    BattleController.instance.UnitObjects[Model.Managers.BattleManager.instance.thisTurnUnit].transform.position
                    + Vector3.up);
        }
    }

}

