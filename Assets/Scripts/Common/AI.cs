using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Model;
using UI.Battle;

namespace Common
{
    public class AI
    {
        /// <summary>
        /// 행동의 우선순위
        /// </summary>
        public enum Priority
        {
            NULL,               // 우선순위 없음
            NearFromMe,         // 나에게 가까운 곳을 우선으로 사용
            FarFromMe,          // 나에게 먼곳을 우선으로 사용
            NearFromPartys,     // 파티들에게 가까운 곳을 우선으로 사용
            FarFromPartys,      // 파티들에게 먼곳을 우선으로 사용
            BiggerCurrentHP,    // 현재HP가 높은 유닛을 우선으로 사용
            SmallerCurrentHP    // 현재HP가 낮은 유닛을 우선으로 사용
        }

        public class Action : MonoBehaviour
        {
            public Unit user;                   // 행동 주체
            public Skill skillToUse;            // 사용할 스킬
            public Vector2Int targetPosition;   // 스킬을 사용할 위치
            public Vector2Int movePosition;     // 이동할 위치

            public Action(Unit user, Skill skillToUse, Vector2Int targetPosition, Vector2Int movePosition)
            {
                this.user = user;
                this.skillToUse = skillToUse;
                this.targetPosition = targetPosition;
                this.movePosition = movePosition;
            }

            public void Invoke()
            {
                StartCoroutine(InvokeAIAction(this));
            }

            public IEnumerator InvokeAIAction(Action action)
            {
                yield return StartCoroutine(action.user.MoveSkill.Use(action.user, action.movePosition));

                if (action.skillToUse != null)
                    yield return StartCoroutine(action.skillToUse.Use(action.user, action.targetPosition));

                BattleUI.instance.TurnEnd();
            }
        }

        /// <summary>
        /// AI 발동
        /// </summary>
        /// <param name="user"></param>
        public static Action GetAction(Unit user)
        {
            // 1. 사용할 스킬을 정한다. 
            // 1.1. 사용할수 있는가? 
            // 1.2. 스킬을 사용할수 있어야함 : 재사용 대기시간이 0이어야하고, 스킬 행동력이 있어야함
            // 1.3. 이동 + 스킬범위 내에 사용할수 있는 유닛이 있어야함)
            // 2. 스킬을 사용할 위치를 정한다. (사용스킬 우선순위에 따름)
            // 3. 스킬을 사용할 수 있는 위치를 역계산한다. (이동스킬 우선순위에 따름)
            // 4. 역계산한 스킬위치들 중에서 하나를 선택한다.

            Skill skillToUse = null;
            int reuseTime = 0;

            foreach (var skill in user.Skills)
            {
                if (skill == null)
                    continue;

                // 사용할수 있는 스킬을 찾는다.
                if (skill.IsUsable(user) && skill.reuseTime >= reuseTime && GetTargetPosition(user, skill) != null)
                    skillToUse = skill;
            }

            Vector2Int targetPosition = (Vector2Int)GetTargetPosition(user, skillToUse);
            Vector2Int movePosiiton = (Vector2Int)GetMovePostion(user, skillToUse, targetPosition);

            return new Action(user, skillToUse, targetPosition, movePosiiton);
        }

        /// <summary>
        /// 이동과 같이 스킬을 사용할수 있는 총 범위
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        private static List<Vector2Int> GetMovedSkillablePositions(Unit user, Skill skill)
        {
            // 이동후 스킬 사용한 모든 위치
            List<Vector2Int> MovedSkillablePositions = new List<Vector2Int>();

            // 이동 가능한 위치
            List<Vector2Int> MoveablePositions = user.MoveSkill.GetAvailablePositions(user);

            foreach (var moveablePosition in MoveablePositions)
            {
                // 이동후 그 위치에서 스킬이 가능한 위치 리스트를 찾는다.
                List<Vector2Int> SkillablePositions = skill.GetAvailablePositions(user, moveablePosition);

                // 스킬을 사용가능한 위치를 순회하면서
                foreach (var skillablePosition in SkillablePositions)
                {
                    // 리스트에 아직 추가 안된 위치라면 추가한다.
                    if (!MovedSkillablePositions.Contains(skillablePosition))
                        MovedSkillablePositions.Add(skillablePosition);
                }
            }

            return MovedSkillablePositions;
        }

        /// <summary>
        /// 타겟에 스킬을 사용할 수 있는 (이동가능한)위치들을 반환한다.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private static List<Vector2Int> GetTargetedMoveablePositions(Unit user, Skill skill, Vector2Int targetPosition)
        {
            // 정해져 있는 타겟에 스킬을 사용할수 있는 이동가능한 위치들
            List<Vector2Int> TargetedMovablePositions = new List<Vector2Int>();

            // 이동가능한 모든 위치들
            List<Vector2Int> MoveablePositions = user.MoveSkill.GetAvailablePositions(user);

            // 이동가능한 위치들을 순회하면서
            foreach (var moveablePosition in MoveablePositions)
            {
                // 이 위치에서 스킬을 사용가능한 위치들을 찾는다.
                List<Vector2Int> SkillablePositions = skill.GetAvailablePositions(user, moveablePosition);

                // 스킬을 사용할 수 있는 위치에 타겟위치가 포함된다면 리스트에 추가한다.
                if (SkillablePositions.Contains(targetPosition))
                    TargetedMovablePositions.Add(moveablePosition);
            }
            return TargetedMovablePositions;
        }

        /// <summary>
        /// 위치들을 받아서 가장 우선순위에 적합한 한 위치를 반환하는 함수 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="positions"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        private static Vector2Int? GetPriorityPosition(Unit user, List<Vector2Int> positions, Priority priority)
        {
            Vector2Int targetPosition;

            if (positions.Count > 0)
                targetPosition = positions[0];
            else
                return null; // 아무데도 사용할수 없다면 null를 리턴합니다.

            for (int i = 1; i < positions.Count; i++)
            {
                // 타겟 우선순위가 나로부터 가장 가까운 위치이다.
                if (priority == Priority.NearFromMe &&
                    (targetPosition - user.Position).magnitude > (positions[i] - user.Position).magnitude)
                    targetPosition = positions[i];
                // 타겟 우선순위가 나로부터 가장 먼 위치이다.
                else if (priority == Priority.FarFromMe &&
                    (targetPosition - user.Position).magnitude < (positions[i] - user.Position).magnitude)
                    targetPosition = positions[i];
                // 타겟 우선순위가 현재 HP가 가장 많은 유닛의 위치이다.
                else if (priority == Priority.BiggerCurrentHP &&
                    Model.Managers.BattleManager.GetUnit(targetPosition).CurrentHP < Model.Managers.BattleManager.GetUnit(positions[i]).CurrentHP)
                    targetPosition = positions[i];
                // 타겟 우선순위가 현재 HP가 가장 적은 유닛의 위치이다.
                else if (priority == Priority.SmallerCurrentHP &&
                    Model.Managers.BattleManager.GetUnit(targetPosition).CurrentHP > Model.Managers.BattleManager.GetUnit(positions[i]).CurrentHP)
                    targetPosition = positions[i];
                else if (priority == Priority.NearFromPartys)
                {
                    Vector2 averagePosition = new Vector2();
                    List<Unit> partyUnits = Model.Managers.BattleManager.GetUnit(Category.Party);

                    foreach (var unit in partyUnits)
                        averagePosition += unit.Position;

                    averagePosition /= partyUnits.Count;



                }
            }

            return targetPosition;
        }

        /// <summary>
        /// 어디에 스킬을 사용할지 위치를 반환하는 함수
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        private static Vector2Int? GetTargetPosition(Unit user, Skill skill)
        {
            List<Vector2Int> TargetPositions = GetMovedSkillablePositions(user, skill);

            return GetPriorityPosition(user, TargetPositions, skill.priority);
        }

        /// <summary>
        /// 정해진 타겟 위치를 기반으로 어디로 이동할지의 위치를 반환하는 함수
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skill"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private static Vector2Int? GetMovePostion(Unit user, Skill skill, Vector2Int targetPosition)
        {
            List<Vector2Int> MovePositions;

            if (skill != null)
                MovePositions = GetTargetedMoveablePositions(user, skill, targetPosition);
            else
                MovePositions = user.MoveSkill.GetAvailablePositions(user);

            return GetPriorityPosition(user, MovePositions, user.MoveSkill.priority);
        }
    }
}