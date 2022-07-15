using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        instance = this;

        foreach (Skill skill in SkillList)
            SkillDictionary.Add(skill.name, skill);

        foreach (Artifact artifact in ArtifactList)
            ArtifactDictionary.Add(artifact.name, artifact);

        foreach (StateEffect stateEffect in StateEffectList)
            StateEffectDictionary.Add(stateEffect.name, stateEffect);
    }

    [Header("이름 : 스킬")]
    public List<Skill> SkillList;
    public StringSkillDictionary SkillDictionary;

    [Header("이름 : 아티펙트")]
    public List<Artifact> ArtifactList;
    public StringArtifactDictionary ArtifactDictionary;

    [Header("이름 : 상태효과")]
    public List<StateEffect> StateEffectList;
    public StringStateEffectDictionary StateEffectDictionary;

    [Header("이름 : 이미지")]
    public StringSpriteDictionary SpriteDictionary;
}
