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

        foreach (Unit unit in UnitList)
            UnitDictionary.Add(unit.name, unit);

        foreach (Skill skill in SkillList)
            SkillDictionary.Add(skill.name, skill);

        foreach (Artifact artifact in ArtifactList)
            ArtifactDictionary.Add(artifact.name, artifact);

        foreach (StateEffect stateEffect in StateEffectList)
            StateEffectDictionary.Add(stateEffect.name, stateEffect);

        foreach (Tile tile in TileList)
            TileDictionary.Add(tile.name, tile);
    }

    [Header("유닛")]
    public List<Unit> UnitList;
    public StringUnitDictionary UnitDictionary;

    [Header("스킬")]
    public List<Skill> SkillList;
    public StringSkillDictionary SkillDictionary;

    [Header("아티펙트")]
    public List<Artifact> ArtifactList;
    public StringArtifactDictionary ArtifactDictionary;

    [Header("상태효과")]
    public List<StateEffect> StateEffectList;
    public StringStateEffectDictionary StateEffectDictionary;

    [Header("이미지")]
    public StringSpriteDictionary SpriteDictionary;


    [Header("타일")]
    public List<Tile> TileList;
    public StringTileDictionary TileDictionary;
}
