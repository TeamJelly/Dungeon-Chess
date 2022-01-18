using Common;
using Model.Managers;
using Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using System.Collections;

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

            if (nextUnit == null)
            {
                Debug.LogError("다음턴을 진행할 유닛이 존재하지 않습니다.");
                return;
            }
                
            BattleManager.SetNextTurnUnit(nextUnit);
            Debug.Log($"{nextUnit.Name}의 턴입니다.");

            // 카메라를 다음 턴 유닛에게 포커스
            ScreenTouchManager.instance.Move(nextUnit.Position);
            
            // 턴시작시 유닛 값들 초기화
            nextUnit.ActionRate = 0;
            nextUnit.IsMoved = false;
            nextUnit.IsSkilled = false;

            // 턴 시작시 스킬쿨 줄어듬
            foreach (var skill in nextUnit.Skills)
            {
                if (skill.WaitingTime > 0)
                    skill.WaitingTime--;
            }
            
            // 턴시작시 발동할게 있으면 등록해둔 이벤트 호출
            nextUnit.OnTurnStart.before.Invoke(false);
            nextUnit.OnTurnStart.after.Invoke(true);

            // 유닛정보창 초기화
            BattleView.SetTurnUnitPanel(nextUnit);

            // 매턴 시작시 DownStair Button 활성화 검사
            // Model.Tiles.DownStair.CheckPartyDownStair();

            // 파티원이 아닌 AI라면 자동 행동 실행
            if (nextUnit.Alliance != UnitAlliance.Party || GameManager.InAuto == true)
            {
                BattleView.TurnEndButton.gameObject.SetActive(false);
                BattleView.DownStairButton.gameObject.SetActive(false);
                BattleView.UnitControlView.panel.SetActive(false);

                AI.Action action = AI.GetAction(nextUnit);

                if (action != null)
                    action.Invoke();
                else if (BattleManager.CheckGameState() == BattleManager.State.Continue)
                    BattleController.instance.ThisTurnEnd();
            }
            else
            {
                BattleView.TurnEndButton.gameObject.SetActive(true);
                BattleView.UnitControlView.panel.SetActive(true);
            }
        }

        public static Coroutine action_coroutine = null;

        public void ThisTurnEnd()
        {
            IndicatorView.HideTileIndicator();
            //Viewer.battle.ThisTurnUnitInfo.CurrentPushedButton = null;
            Unit thisTurnUnit = BattleManager.instance.thisTurnUnit;

            if(thisTurnUnit != null)
            {
                thisTurnUnit.OnTurnEnd.before.Invoke(true);
                thisTurnUnit.OnTurnEnd.after.Invoke(false);
            }

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
                case BattleManager.State.Stop:
                    break;
            }
        }

        public static void SetBattleMode(bool b)
        {
            BattleManager.instance.thisTurnUnit = null;
            GameManager.InBattle = b;
            BattleView.TurnEndButton.gameObject.SetActive(b);
            UnitControlView.instance.skillPanel.SetActive(b);
        }

        public void Win()
        {
            BattleView.DownStairButton.gameObject.SetActive(true);
            SetBattleMode(false);
        }

        public void Defeat()
        {
            // View.BattleResultView.instance.EnableDeafeatUI();
        }
    }
}