using UnityEditor;
using UnityEngine;
using Model;
using Model.Managers;
using System.Collections.Generic;
using UI.Battle;
using View;

namespace Common
{
    public class Command
    {
        public static void Die(Unit unit)
        {
            View.FadeOutTextView.MakeText(unit.Position, unit.Name + " is dead", Color.red);

            unit.Alliance = UnitAlliance.NULL;

            View.VisualEffectView.MakeVisualEffect(unit.Position, "Explosion");
            UnSummon(unit);

            // 소지 유물 뿌리기
            List<Tile> tiles = FieldManager.GetBlankFloorTiles(unit.Belongings.Count);
            int count = 0;
            foreach(Tile tile in tiles)
            {
                Summon(unit.Belongings[count], tile.position);
                Vector3 startPos = new Vector3(unit.Position.x, unit.Position.y, 0.1f);
                Vector3 target = new Vector3(tile.position.x, tile.position.y, 0.1f);
                VisualEffectView.MakeDropEffect(startPos,target,unit.Belongings[count]);
                count++;
            }

            GameManager.RemovePartyUnit(unit); //죽으면 파티유닛에서 박탈.

            BattleManager.instance.InitializeUnitBuffer();

            //if (BattleManager.CheckGameState() != BattleManager.State.Continue)
            if (GameManager.InBattle)
                BattleController.instance.ThisTurnEnd();

            else if (BattleManager.instance.thisTurnUnit == unit) 
                BattleManager.instance.thisTurnUnit = null;
        }

        // position 이동보다 좀더 확장된 이동
        // 타일에 유닛의 정보를 기록하고 유닛의 OnMove, 타일의 OnTile 이벤트를 실행시킨다.
        public static void Move(Unit unit, Vector2Int start, Vector2Int target)
        {
            //OnMove는 이동 할때마다 항상 수행되는 이벤트
            target = unit.OnMove.before.Invoke(target);

            FieldManager.GetTile(start).SetUnit(null);
            FieldManager.GetTile(target).SetUnit(unit);
            unit.Position = target;
            unit.OnMove.after.Invoke(target);

            //OnTile은 타일의 특성에 따라 이동이 끝난 후 발동되는 타일의 이벤트
            FieldManager.GetTile(target).OnTile(unit);
        }

        public static int Damage(Unit unit, int value)
        {
            value = unit.OnDamage.before.Invoke(value);

            unit.CurHP -= value;
            unit.OnDamage.after.Invoke(value);

            FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"HP -{value}", Color.red);
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

            FadeOutTextView.MakeText(unit.Position + Vector2Int.up, $"HP +{value}", Color.green);

            Debug.Log($"{unit.Name}가(은) {value}만큼 회복했다! [HP : {unit.CurHP}>{unit.CurHP + value}]");

            return value;
        }

        public static void LevelUp(Unit unit)
        {
            View.FadeOutTextView.MakeText(unit.Position + Vector2Int.up, "Level Up!", Color.white);

            unit.Level++;

            unit.CurEXP = 0;
            unit.NextEXP = 10 * unit.Level * (unit.Level + 5);            
        }
        
        public static void AddArtifact(Unit target, Artifact artifact)
        {
            artifact.Owner = target;
            artifact.OnAdd();
            target.Belongings.Add(artifact);
            FadeOutTextView.MakeText(target.Position + Vector2Int.up, $"+{artifact.Name}", Color.yellow);
        }

        public static void RemoveArtifact(Unit target, Artifact artifact)
        {
            if (target.Belongings.Contains(artifact))
            {
                artifact.OnRemove();
                target.Belongings.Remove(artifact);
                FadeOutTextView.MakeText(target.Position + Vector2Int.up, $"-{artifact.Name}", Color.yellow);
            }
            else
                Debug.LogError($"{target.Name}이 {artifact.Name}를 소유하고 있지 않습니다.");
        }

        // public static Effect GetEffectByNumber(Unit unit, int value)
        // {
        //     Effect effect = null;

        //     foreach (var stateEffect in unit.StateEffects)
        //         if (stateEffect.Number == value)
        //             effect = stateEffect;

        //     return effect;
        // }

        public static void AddEffect(Unit target, Effect effect)
        {
            effect.OnAdd();
            target.StateEffects.Add(effect);
            FadeOutTextView.MakeText(target.Position + Vector2Int.up, $"+{effect.Name}", Color.yellow);
        }

        public static void RemoveEffect(Unit target, Effect effect)
        {
            if (target.StateEffects.Contains(effect))
            {
                effect.OnRemove();
                target.StateEffects.Remove(effect);
                FadeOutTextView.MakeText(target.Position + Vector2Int.up, $"-{effect.Name}", Color.yellow);
            }
            else
                Debug.LogError($"{target.Name}이 {effect.Name}를 소유하고 있지 않습니다.");
        }

        public static void Summon(Unit unit, Vector2Int target)
        {
            if (BattleManager.GetUnit(target) == null)
            {
                unit.OnPosition.after.RemoveListener(BattleView.MoveObject);
                unit.Position = target;
                FieldManager.GetTile(target).SetUnit(unit);
                BattleManager.instance.AllUnits.Add(unit);
                BattleView.MakeUnitObject(unit);

                FieldManager.GetTile(target).OnTile(unit);
            }
            else
                Debug.LogError("이미 위치에 유닛이 존재합니다.");
        }

        // 최초 Obatainable 최초 소환
        public static void Summon(Obtainable obtainable, Vector2Int position)
        {
            FieldManager.GetTile(position).SetObtainable(obtainable);
            obtainable.Position = position;
            BattleManager.instance.AllObtainables.Add(obtainable);
            BattleView.MakeObtainableObject(obtainable, position);
        }

        public static void UnSummon(Unit unit)
        {
            BattleView.DestroyUnitObject(unit);
            BattleManager.instance.AllUnits.Remove(unit);
            FieldManager.GetTile(unit.Position).SetUnit(null);
        }
        public static void UnSummon(Obtainable obtainable)
        {
            FieldManager.GetTile(obtainable.Position).SetObtainable(null);
            BattleView.DestroyObtainableObject(obtainable);
        }
        
        public static void UnSummonAllUnit()
        {
            List<Unit> units = BattleManager.instance.AllUnits;
            for (int i = units.Count - 1; i >= 0; i--)
            {
                UnSummon(units[i]);
            }
        }

        // public static void AddSkill(Unit unit, Skill newSkill, int index)
        // {
        //     if (index >= unit.Skills.Length || index < 0)
        //     {
        //         Debug.LogError("스킬 슬롯을 범위를 벗어났습니다.");
        //         return;
        //     }

        //     RemoveSkill(unit, index);
        //     unit.Skills[index] = newSkill;
        //     // unit.IsModified = true;
        // }

        // public static void RemoveSkill(Unit unit, int index)
        // {
        //     if (index >= unit.Skills.Length || index < 0)
        //     {
        //         Debug.LogError("스킬 슬롯을 범위를 벗어났습니다.");
        //         return;
        //     }
        //     if (unit.Skills[index] == null)
        //     {
        //         Debug.LogError("제거할 스킬이 없습니다.");
        //         return;
        //     }

        //     unit.Skills[index] = null;
        // }

        // public static void UpgradeSkill(Unit unit, int index)
        // {
        //     if (index >= unit.Skills.Length || index < 0)
        //     {
        //         Debug.LogError("스킬 슬롯을 범위를 벗어났습니다.");
        //         return;
        //     }
        //     if (unit.Skills[index] == null)
        //     {
        //         Debug.LogError("업그레이드 할 스킬이 없습니다.");
        //         return;
        //     }
        //     unit.Skills[index].Upgrade();
        // }

    }
}