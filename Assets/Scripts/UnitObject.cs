using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    // void LoadUnitData(Unit u)
    // {
    //     unit = u;

    //     Alliance = (UnitAlliance)u.Alliance;
    //     Species = (UnitSpecies)u.Species;
    //     Modifier = (UnitModifier)u.Modifier;

    //     IsSkilled = u.IsSkilled;
    //     IsMoved = u.IsMoved;
    //     isFlying = u.IsFlying;

    //     skills.Clear();
    //     stateEffects.Clear();
    //     belongings.Clear();

    //     foreach (Skill s in skills)
    //     {
    //         Common.Command.RemoveSkill(this, s);
    //     }
    //     foreach (Effect e in stateEffects)
    //     {
    //         Common.Command.RemoveEffect(this, e);
    //     }
    //     foreach (Obtainable o in belongings)
    //     {
    //         Common.Command.RemoveArtifact(this, (Artifact)o);
    //     }

    //     for (int i = 0; i < u.skills.Count; i++)
    //     {
    //         Skill skl = (Skill)Activator.CreateInstance(Type.GetType(u.skills[i]));
    //         skl.WaitingTime = u.skill_waitingTimes[i];
    //         Common.Command.UpgradeSkill(skl, u.skill_levels[i]);
    //         this.AddSkill(skl);
    //     }

    //     for (int i = 0; i < u.stateEffects.Count; i++)
    //     {
    //         Common.Command.AddEffect(this, (Effect)Activator.CreateInstance(Type.GetType(u.stateEffects[i])));
    //     }

    //     foreach (string o in u.belongings)
    //     {
    //         Common.Command.AddArtifact(this, (Artifact)Activator.CreateInstance(Type.GetType(o)));
    //     }

    //     //유물 영향 받는 스텟 마지막에 갱신.
    //     curHP = u.curHP;
    //     maxHP = u.maxHP;
    //     strength = u.strength;
    //     agility = u.agility;
    //     mobility = u.mobility;
    //     criRate = u.criRate;
    //     actionRate = u.actionRate;
    // }

    public Sprite sprite => GetComponent<SpriteRenderer>().sprite;

    public UnitData unit;

    // public List<Skill> Skills;
    // public List<
    // 유닛 애니메이션 함수도 필요
}