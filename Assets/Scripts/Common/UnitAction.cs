using UnityEditor;
using UnityEngine;
using Model;
using Model.Managers;
using System.Collections.Generic;

namespace Common
{
    public class UnitAction
    {
        public static void Die(Unit unit)
        {
            Debug.Log($"{unit.Name}는(은) 사망했다!");
            unit.Agility = -10;
        }

        public static void Move(Unit unit, Vector2Int target)
        {
            // BeforeMove();

            //기존 타일 유닛 초기화 Tile.cs로 옮기면 좋을듯
            BattleManager.GetTile(unit.Position).SetUnit(null);
            unit.Position = target;
            BattleManager.GetTile(target).SetUnit(unit);

            // AfterMove();
        }

        public static int Damage(Unit unit, int number)
        {
            Debug.Log($"{unit.Name}가(은) {number}만큼 데미지를 입었다! [HP : {unit.CurrentHP}>{unit.CurrentHP - number}]");

            //BeforeDamage(unit, number);

            unit.CurrentHP -= number;

            //AfterDamage(unit, number);

            if (unit.CurrentHP <= 0)
                Die(unit);

            return number; // 피해량을 리턴
        }

        public static int Heal(Unit unit, int number)
        {
            // Before Heal

            unit.CurrentHP += number;

            if (unit.MaximumHP < unit.CurrentHP)
                unit.CurrentHP = unit.MaximumHP;

            // After Heal

            return number;
        }

        public static void LevelUp(Unit unit)
        {
            unit.Level++;
            unit.CurrentEXP = 0;
            unit.NextEXP = unit.Level * 10;
            Debug.Log($"{unit.Name} Level Up! ( Lv {unit.Level-1} > Lv {unit.Level} )");
        }

        public static Effect GetEffectByNumber(Unit unit, int number)
        {
            Effect effect = new Effect();

            foreach (var stateEffect in unit.StateEffects)
                if (stateEffect.number == number)
                    effect = stateEffect;

            return effect;
        }

        public static void AddEffect(Unit target, Effect effect)
        {
            effect.OnAddThisEffect();
            target.StateEffects.Add(effect);
        }

        public static void RemoveEffect(Unit unit, Effect effect)
        {
            if (unit.StateEffects.Contains(effect))
            {
                effect.OnRemoveThisEffect();
                unit.StateEffects.Remove(effect);
            }
        }
        
        public static void Summon(Unit unit)
        {
            Summon(unit, unit.Position);
        }

        public static void Summon(List<Unit> units)
        {
            foreach (var unit in units)
                Summon(unit);
        }

        public static void Summon(Unit unit, Vector2Int target)
        {
            if (BattleManager.GetUnit(target) == null)
            {
                unit.Position = target;
                BattleManager.GetTile(target).SetUnit(unit);
                BattleManager.instance.AllUnits.Add(unit);
            }
            else
                Debug.LogError("이미 위치에 유닛이 존재합니다.");
        }

        public static void AddSkill(Unit unit, Skill newSkill, int index)
        {
            if (index >= unit.Skills.Length || index < 0)
            {
                Debug.LogError("스킬 슬롯을 범위를 벗어났습니다.");
                return;
            }

            RemoveSkill(unit, index);
            unit.Skills[index] = newSkill;
        }

        public static void RemoveSkill(Unit unit, int index)
        {
            if (index >= unit.Skills.Length || index < 0)
            {
                Debug.LogError("스킬 슬롯을 범위를 벗어났습니다.");
                return;
            }
            if (unit.Skills[index] == null)
            {
                Debug.LogError("제거할 스킬이 없습니다.");
                return;
            }

            unit.Skills[index] = null;
        }

    }
}