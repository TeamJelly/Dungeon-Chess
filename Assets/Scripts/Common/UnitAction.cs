using UnityEditor;
using UnityEngine;
using Model;
using Model.Managers;

namespace Common
{
    public class UnitAction
    {
        public static void Die(Unit unit)
        {
            unit.agility = 0;
        }

        public static void Move(Unit unit, Vector2Int target)
        {
            //BeforeMove();

            //기존 타일 유닛 초기화 Tile.cs로 옮기면 좋을듯
            BattleManager.GetTile(unit.position).SetUnit(null);
            unit.position = target;
            BattleManager.GetTile(target).SetUnit(unit);

            //AfterMove();
        }

        public static int Damage(Unit unit, int number)
        {
            //BeforeDamage(unit, number);

            unit.currentHP -= number;

            //AfterDamage(unit, number);

            if (unit.currentHP <= 0)
                Die(unit);

            return number; // 피해량을 리턴
        }

        public static int Heal(Unit unit, int number)
        {
            // Before Heal

            unit.currentHP += number;

            if (unit.maxHP < unit.currentHP)
                unit.currentHP = unit.maxHP;

            // After Heal

            return number;
        }

        public static void LevelUp(Unit unit)
        {
            unit.level++;
            unit.currentEXP = 0;
            unit.nextEXP = unit.level * 10;
            Debug.Log(unit.name + " level Up! (Lv" + (unit.level-1) + " > Lv" + unit.level);
        }

        public static void GetEffect(Unit unit, Effect effect)
        {

        }

        public static void Summon(Unit unit, Vector2Int target)
        {
            if (BattleManager.GetUnit(target) == null)
            {
                unit.position = target;
                BattleManager.GetTile(target).SetUnit(unit);
            }
            else
                Debug.LogError("이미 위치에 유닛이 존재합니다.");
        }
    }
}