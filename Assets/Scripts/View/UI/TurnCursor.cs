using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using DG.Tweening;


namespace View.UI
{
    public class TurnCursor : MonoBehaviour
    {
        private void Start()
        {
            GetComponentInChildren<SpriteRenderer>().DOColor(Color.white, 0.5f).From(Color.green).SetLoops(-1, LoopType.Yoyo);
        }

        void FixedUpdate()
        {
            if (Model.Managers.BattleManager.instance.thisTurnUnit != null)
            {
                // GetComponentInChildren<SpriteRenderer>().color = Color.green;
                transform.position = (Vector3Int)(Model.Managers.BattleManager.instance.thisTurnUnit.Position) + new Vector3(0, 1, -2);
            }
            // else
            // {
            //     GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            // }
        }
    }
}