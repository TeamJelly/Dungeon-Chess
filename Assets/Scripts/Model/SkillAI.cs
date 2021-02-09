using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Model
{
    public class SkillAI
    {
        Unit user;
        Skill skill;
        Priority targetPriority = Priority.NearFromMe;

        public enum Priority
        {
            NULL,
            NearFromMe,
            FarFromMe,
            BiggerCurrentHP,
            SmallerCurrentHP    
        }

        SkillAI(Unit user, Skill skill, Priority priority)
        {
            this.user = user;
            this.skill = skill;
            targetPriority = priority;
        }

        public Vector2Int GetTargetPosition(Unit user, Skill skill, Priority targetPriority)
        {
            List<Vector2Int> TargetPositions = new List<Vector2Int>();
            Vector2Int targetPosition;

            foreach (var position in GetMovedSkillablePositions(user, skill))
            {
                Unit positionUnit = Managers.BattleManager.GetUnit(position);

                if (skill.target == Skill.Target.Any && positionUnit != null)
                    TargetPositions.Add(position);
                else if (skill.target == Skill.Target.NoUnit && positionUnit == null)
                    TargetPositions.Add(position);
                else if (skill.target == Skill.Target.Party && positionUnit?.Category == Category.Party)
                    TargetPositions.Add(position);
                else if (skill.target == Skill.Target.Friendly &&
                    (positionUnit?.Category == Category.Friendly || positionUnit?.Category == Category.Party))
                    TargetPositions.Add(position);
                else if (skill.target == Skill.Target.Enemy && positionUnit?.Category == Category.Enemy)
                    TargetPositions.Add(position);
            }

            if (TargetPositions.Count > 0)
                targetPosition = TargetPositions[0];
            else
                return Vector2Int.left + Vector2Int.down; // 아무데도 사용할수 없다면 -1, -1를 리턴합니다.

            for (int i = 1; i < TargetPositions.Count; i++)
            {
                if (targetPriority == Priority.NearFromMe &&
                    (targetPosition - user.Position).magnitude > (TargetPositions[i] - user.Position).magnitude)
                    targetPosition = TargetPositions[i];
                else if (targetPriority == Priority.FarFromMe &&
                    (targetPosition - user.Position).magnitude < (TargetPositions[i] - user.Position).magnitude)
                    targetPosition = TargetPositions[i];
                else if (targetPriority == Priority.BiggerCurrentHP &&
                    (Managers.BattleManager.GetUnit(targetPosition).CurrentHP < Managers.BattleManager.GetUnit(TargetPositions[i]).CurrentHP))
                    targetPosition = TargetPositions[i];
                else if (targetPriority == Priority.SmallerCurrentHP &&
                    (Managers.BattleManager.GetUnit(targetPosition).CurrentHP > Managers.BattleManager.GetUnit(TargetPositions[i]).CurrentHP))
                    targetPosition = TargetPositions[i];
            }

            return targetPosition;
        }

        public List<Vector2Int> GetMovedSkillablePositions(Unit user, Skill skill)
        {
            // 이동과 같이 스킬을 사용할수 있는 총 범위
            List<Vector2Int> MovedSkillablePositions = new List<Vector2Int>();

            List<Vector2Int> MoveablePositions = user.MoveSkill.GetAvailablePositions(user);
            
            foreach (var moveablePosition in MoveablePositions)
            {
                List<Vector2Int> SkillablePositions = skill.GetAvailablePositions(user, user.Position);
                foreach (var skillablePosition in SkillablePositions)
                {
                    Vector2Int temp = moveablePosition + skillablePosition;
                    if (!MovedSkillablePositions.Contains(temp))
                        MovedSkillablePositions.Add(temp);
                }
            }

            return MovedSkillablePositions;
        }


        // 1. 스킬을 사용할 위치를 정한다.
        // 2. 스킬을 사용할수 있는 위치를 역계산한다.
        // 3. 역계산한 스킬위치들 중에서 하나를 선택한다.
    }
}