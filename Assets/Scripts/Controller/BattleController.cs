using Common;
using Model.Managers;
using Model;
using UI.Popup;
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

            // 적 유닛의 뷰를 만든다. -> UnitAction.Summon 으로 이동
            //foreach (var unit in BattleManager.GetUnit())
            //{
            //    ViewManager.battle.MakeUnitObject(unit);
            //}

            // 유닛 배치가 끝난 뒤에 최초 턴을 시작하도록 바꾼다.
            // NextTurnStart();
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
            nextUnit.MoveCount = 1;
            nextUnit.SkillCount = 1;

            // 턴 시작시 스킬쿨 줄어듬
            foreach (var skill in nextUnit.Skills)
                if (skill != null && skill.CurReuseTime != 0)
                    skill.CurReuseTime--;

            //// 뒤에서부터 돌면 중간에 삭제해도 문제 없음.
            //for (int i = nextUnit.StateEffects.Count - 1; i >= 0; i--)
            //    nextUnit.StateEffects[i].OnTurnStart();
            bool _bool = false;
            nextUnit.OnTurnStart.before.RefInvoke(ref _bool);
            _bool = true;
            nextUnit.OnTurnStart.after.RefInvoke(ref _bool);

            // 유닛정보창 초기화
            BattleView.SetTurnUnitPanel(nextUnit);

            // 파티원이 아닌 AI라면 자동 행동 실행
            if (nextUnit.Alliance != UnitAlliance.Party)
            {
                BattleView.TurnEndButton.gameObject.SetActive(false);
                BattleView.UnitControlUI.panel.SetActive(false);
                AI.Action action = AI.GetAction(nextUnit);

                if (action != null)
                    action.Invoke();
            }

            else
            {
                BattleView.TurnEndButton.gameObject.SetActive(true);
                BattleView.UnitControlUI.panel.SetActive(true);
            }
        }

        public void ThisTurnEnd()
        {
            IndicatorUI.HideTileIndicator();
            //Viewer.battle.ThisTurnUnitInfo.CurrentPushedButton = null;

            Unit thisTurnUnit = BattleManager.instance.thisTurnUnit;

            //// 턴 종료 효과 처리
            //for (int i = thisTurnUnit.StateEffects.Count - 1; i >= 0; i--)
            //    thisTurnUnit.StateEffects[i].OnTurnEnd();

            bool _bool = false;
            thisTurnUnit.OnTurnEnd.before.RefInvoke(ref _bool);
            _bool = true;
            thisTurnUnit.OnTurnEnd.after.RefInvoke(ref _bool);

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
            BattleResultUI.instance.EnableWinUI();
        }

        public void Defeat()
        {
            BattleResultUI.instance.EnableDeafeatUI();
        }
    }
}