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
            Viewer.battle.SetTurnEndButton(ThisTurnEnd);
            foreach (var unit in BattleManager.GetUnit())
            {
                Viewer.battle.MakeUnitObject(unit);
                AgilityViewer.instance.AddToList(unit);
            }
            NextTurnStart();
        }

        /// <summary>
        /// 인자로 유닛을 넣지 않을면 자동으로 다음 유닛을 계산하여 다음턴으로 설정한다.
        /// </summary>
        public void NextTurnStart()
        {
            // 다음 턴의 유닛을 받아 시작한다.
            Unit nextUnit = BattleManager.GetNextTurnUnit();
            BattleManager.SetNextTurnUnit(nextUnit);
            AgilityViewer.instance.UpdteView();
            // 턴시작시 유닛 값들 초기화
            nextUnit.ActionRate = 0;
            nextUnit.MoveCount = 1;
            nextUnit.SkillCount = 1;
            nextUnit.ItemCount = 1;

            // 턴 시작시 스킬쿨 줄어듬
            foreach (var skill in nextUnit.Skills)
                if (skill != null && skill.CurrentReuseTime != 0)
                    skill.CurrentReuseTime--;

            // 뒤에서부터 돌면 중간에 삭제해도 문제 없음.
            for (int i = nextUnit.StateEffects.Count - 1; i >= 0; i--)
                nextUnit.StateEffects[i].OnTurnStart();

            // 유닛정보창 초기화
            Viewer.battle.SetTurnUnitPanel(nextUnit);

            // AI라면 자동 행동 실행
            if (nextUnit.Category != Category.Party)
            {
                AI.Action action = AI.GetAction(nextUnit);

                if (action != null)
                    action.Invoke();
            }

        }

        public void ThisTurnEnd()
        {
            IndicatorUI.HideTileIndicator();
            Viewer.battle.ThisTurnUnitInfo.CurrentPushedButton = null;

            Unit thisTurnUnit = BattleManager.instance.thisTurnUnit;

            // 턴 종료 효과 처리
            for (int i = thisTurnUnit.StateEffects.Count - 1; i >= 0; i--)
                thisTurnUnit.StateEffects[i].OnTurnEnd();

            switch(BattleManager.CheckGameState())
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