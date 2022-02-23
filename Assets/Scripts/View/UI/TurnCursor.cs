using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using DG.Tweening;
using Model.Managers;

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
                transform.position = BattleView.UnitObjects[BattleManager.instance.thisTurnUnit].transform.position + new Vector3(0, 1, -2);
        }
    }
}