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
            unit.Category = Category.NULL;

            VisualEffectUI.MakeVisualEffect(unit.Position, "explosion");


            ViewManager.battle.DestroyUnitObject(unit);
            BattleManager.instance.AllUnits.Remove(unit);            
            BattleManager.GetTile(unit.Position).SetUnit(null);
            BattleManager.instance.InitializeUnitBuffer();

            if (BattleManager.CheckGameState() != BattleManager.State.Continue)
                BattleController.instance.ThisTurnEnd();
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
            for (int i = unit.StateEffects.Count - 1; i >= 0; i--)
            {
                number = unit.StateEffects[i].BeforeGetDamage(number);
            }

            string text = "";

            if (unit.Armor > 0)
            {
                int damagedArmor = unit.Armor - number;

                if (damagedArmor > 0)
                {
                    text += $"방어함!\n";
                    text += $"Armor -{number}";

                    unit.Armor = damagedArmor;
                    number = 0;
                }
                else
                {
                    text += $"Armor -{unit.Armor}\n";
                    text += $"HP -{-damagedArmor}";

                    unit.Armor = 0;
                    number = -damagedArmor;
                }
            }
            else
            {
                text += $"HP -{number}";
            }

            unit.CurrentHP -= number;

            FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, text, Color.red);

            Debug.Log($"{unit.Name}가(은) {number}만큼 데미지를 입었다! [HP : {unit.CurrentHP + number}>{unit.CurrentHP}]");

            if (unit.CurrentHP <= 0)
                Die(unit);

            
            return number; // 피해량을 리턴
        }

        public static int Heal(Unit unit, int number)
        {
            // Before Heal


            if (unit.CurrentHP + number > unit.MaximumHP)
                number = unit.MaximumHP - unit.CurrentHP;

            unit.CurrentHP += number;

            FadeOutTextUI.MakeText(unit.Position + Vector2Int.up, $"HP +{number}", Color.green);

            Debug.Log($"{unit.Name}가(은) {number}만큼 회복했다! [HP : {unit.CurrentHP}>{unit.CurrentHP + number}]");

            // After Heal

            return number;
        }

        public static int Armor(Unit unit, int number)
        {
            unit.Armor += number;
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
            Effect effect = null;

            foreach (var stateEffect in unit.StateEffects)
                if (stateEffect.Number == number)
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