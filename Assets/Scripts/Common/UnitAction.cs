using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;
using Model.Managers;
using System.Collections.Generic;
using UI.Battle;
using View;

namespace Common
{
    public class UnitAction
    {
        public static void Die(Unit unit)
        {
            Debug.Log($"{unit.Name}는(은) 사망했다!");

            unit.Agility = -10;
            unit.Alliance = UnitAlliance.NULL;

            VisualEffectView.MakeVisualEffect(unit.Position, "Explosion");

            ViewManager.battle.DestroyUnitObject(unit);
            BattleManager.instance.AllUnits.Remove(unit);            
            FieldManager.GetTile(unit.Position).SetUnit(null);
            BattleManager.instance.InitializeUnitBuffer();

            if (BattleManager.CheckGameState() != BattleManager.State.Continue)
                BattleController.instance.ThisTurnEnd();
        }

        public static void Move(Unit unit, Vector2Int target)
        {
            // BeforeMove();

            //기존 타일 유닛 초기화 Tile.cs로 옮기면 좋을듯
            FieldManager.GetTile(unit.Position).SetUnit(null);
            unit.Position = target;
            FieldManager.GetTile(target).SetUnit(unit);

            // AfterMove();
        }

        public static int Damage(Unit unit, int value)
        {
            value = unit.OnDamage.before.Invoke(value);

            if (unit.Armor > 0)
            {
                int damagedArmor = unit.Armor - value;

                if (damagedArmor > 0)
                {
                    FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, "방어함!", Color.red);
                    FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, $"Armor -{value}", Color.red);

                    unit.Armor = damagedArmor;
                    value = 0;
                }
                else
                {
                    FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, $"Armor -{unit.Armor}", Color.red);
                    FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, $"HP -{-damagedArmor}", Color.red);

                    unit.Armor = 0;
                    value = -damagedArmor;
                }
            }
            else
                FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, $"HP -{value}", Color.red);

            unit.CurHP -= value;
            unit.OnDamage.after.Invoke(value);

            Debug.Log($"{unit.Name}가(은) {value}만큼 데미지를 입었다! [HP : {unit.CurHP + value}>{unit.CurHP}]");

            if (unit.CurHP <= 0)
                Die(unit);

            
            return value; // 피해량을 리턴
        }

        public static int Heal(Unit unit, int value)
        {
            value = unit.OnHeal.before.Invoke(value);

            // 최대체력 이상으로 회복하지 않습니다.
            if (unit.CurHP + value > unit.MaxHP)
                value = unit.MaxHP - unit.CurHP;

            unit.CurHP += value;
            unit.OnHeal.after.Invoke(value);

            FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, $"HP +{value}", Color.green);

            Debug.Log($"{unit.Name}가(은) {value}만큼 회복했다! [HP : {unit.CurHP}>{unit.CurHP + value}]");



            return value;
        }

        public static int Armor(Unit unit, int value)
        {
            unit.Armor += value;
            return value;
        }
        public static void LevelUp(Unit unit)
        {
            unit.Level++;
            unit.CurEXP = 0;
            unit.NextEXP = unit.Level * 10;
            Debug.Log($"{unit.Name} Level Up! ( Lv {unit.Level-1} > Lv {unit.Level} )");
        }

        public static Effect GetEffectByNumber(Unit unit, int value)
        {
            Effect effect = null;

            foreach (var stateEffect in unit.StateEffects)
                if (stateEffect.Number == value)
                    effect = stateEffect;

            return effect;
        }

        public static void AddEffect(Unit target, Effect effect)
        {
            effect.OnAddThisEffect();
            target.StateEffects.Add(effect);
            FadeOutTextUI.MakeText(target.Position + Vector2Int.up, $"+{effect.Name}", Color.yellow);
        }

        public static void RemoveEffect(Unit target, Effect effect)
        {
            if (target.StateEffects.Contains(effect))
            {
                effect.OnRemoveThisEffect();
                target.StateEffects.Remove(effect);
                FadeOutTextUI.MakeText(target.Position + Vector2Int.up, $"-{effect.Name}", Color.yellow);
            }
            else
                Debug.LogError($"{target.Name}이 {effect.Name}를 소유하고 있지 않습니다.");
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
                FieldManager.GetTile(target).SetUnit(unit);
                BattleManager.instance.AllUnits.Add(unit);
                ViewManager.battle.MakeUnitObject(unit);
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
            // unit.IsModified = true;
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

        public static void EnhanceSkill(Unit unit, int index)
        {
            if (index >= unit.Skills.Length || index < 0)
            {
                Debug.LogError("스킬 슬롯을 범위를 벗어났습니다.");
                return;
            }
            if (unit.Skills[index] == null)
            {
                Debug.LogError("업그레이드 할 스킬이 없습니다.");
                return;
            }
            unit.Skills[index].Upgrade();
        }
    }
}