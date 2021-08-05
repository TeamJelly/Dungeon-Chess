using Common;
using Model.Managers;
using Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View;

namespace UI.Battle
{
    public class BattleController : MonoBehaviour
    {
        public static BattleController instance;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            BattleView.SetTurnEndButton(ThisTurnEnd);
        }

        /// <summary>
        /// 인자로 유닛을 넣지 않을면 자동으로 다음 유닛을 계산하여 다음턴으로 설정한다.
        /// </summary>
        public void NextTurnStart()
        {
            // 다음 턴의 유닛을 받아 시작한다.
            Unit nextUnit = BattleManager.GetNextTurnUnit();
            BattleManager.SetNextTurnUnit(nextUnit);
            
            // 턴시작시 유닛 값들 초기화
            nextUnit.ActionRate = 0;
            nextUnit.IsMoved = false;
            nextUnit.IsSkilled = false;

            // 턴 시작시 스킬쿨 줄어듬
            foreach (var skill in new List<Skill>(nextUnit.WaitingSkills.Keys))
            {
                nextUnit.WaitingSkills[skill] -= 1;
                if (nextUnit.WaitingSkills[skill] <= 0)
                    nextUnit.WaitingSkills.Remove(skill);
            }
            
            nextUnit.OnTurnStart.before.Invoke(false);
            nextUnit.OnTurnStart.after.Invoke(true);

            // 유닛정보창 초기화
            BattleView.SetTurnUnitPanel(nextUnit);

            // 파티원이 아닌 AI라면 자동 행동 실행
            if (nextUnit.Alliance != UnitAlliance.Party)
            {
                BattleView.TurnEndButton.gameObject.SetActive(false);
                BattleView.UnitControlView.panel.SetActive(false);
                AI.Action action = AI.GetAction(nextUnit);

                if (action != null)
                    action.Invoke();
            }

            else
            {
                BattleView.TurnEndButton.gameObject.SetActive(true);
                BattleView.UnitControlView.panel.SetActive(true);
            }
        }

        public void ThisTurnEnd()
        {
            IndicatorView.HideTileIndicator();
            //Viewer.battle.ThisTurnUnitInfo.CurrentPushedButton = null;

            Unit thisTurnUnit = BattleManager.instance.thisTurnUnit;

            thisTurnUnit.OnTurnEnd.before.Invoke(true);
            thisTurnUnit.OnTurnEnd.after.Invoke(false);

            switch (BattleManager.CheckGameState())
            {
                case BattleManager.State.Continue:
                    NextTurnStart();
                    break;
                case BattleManager.State.Win:
                    Win();
                    break;
                case BattleManager.State.Defeat:
                    Defeat();
                    break;
            }
        }

        public void Win()
        {
            Common.Command.UnSummonAllUnit();
            BattleView.SetNonBattleMode();
        }

        public void Defeat()
        {
            View.BattleResultView.instance.EnableDeafeatUI();
        }
    }
}