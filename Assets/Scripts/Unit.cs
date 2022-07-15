using UnityEngine;
using System.Collections.Generic;
using Common;
using System;
using Model;

/// <summary>
/// 진영 : 유닛이 속한 진영을 결정한다. 전투중 행동방식을 결정
/// </summary>
public enum UnitAlliance
{
    NULL = -1,
    Party,      // 파티원
    Enemy,      // 적 AI
    Neutral,    // 중립 AI
    Friendly,   // 우호적 AI
};

[Serializable]
public class Unit : ISpriteable
{
    #region 인적정보
    [SerializeField] private int id;                             // 유닛 고유 ID
    [SerializeField] private string name;
    [SerializeField] private string sprite_name;
    [SerializeField] private UnitAlliance alliance;
    #endregion

    #region 레벨
    [SerializeField] private int level = 1;
    [SerializeField] private int ability_point;                   // 레벨업후 능력치
    #endregion

    #region 경험치
    [SerializeField] private int cur_exp;                         // 현재 경험치
    [SerializeField] private int next_exp;                        // 다음 레벨업에 필요한 경험치 
    #endregion

    #region 체력
    [SerializeField] private int cur_hp;                          // 현재 체력
    [SerializeField] private int max_hp;                          // 최대 체력
    #endregion

    #region 스테이터스
    [SerializeField] private int vitality;                        // 생명력
    [SerializeField] private int mobility;                       // 이동력
    [SerializeField] private int strength;                       // 힘
    [SerializeField] private int action;                         // 행동가능 횟수
    #endregion

    #region 위치
    [SerializeField] private int position_x;                // 위치
    [SerializeField] private int position_y;
    #endregion

    [SerializeField] private List<string> skills = new List<string>(5);
    [SerializeField] private StringIntDictionary skill_waiting_time = new StringIntDictionary();
    [SerializeField] private StringIntDictionary skill_level = new StringIntDictionary();

    [SerializeField] private List<string> artifacts = new List<string>();         // 보유한 유물
    [SerializeField] private StringIntDictionary artifact_count = new StringIntDictionary();

    [SerializeField] private List<string> state_effects = new List<string>(); // 보유한 상태효과
    [SerializeField] private StringIntDictionary state_effect_count = new StringIntDictionary();

    [SerializeField] private int skill_count_this_turn;
    [SerializeField] private int move_count_this_turn;
    [SerializeField] private bool is_skillable;
    [SerializeField] private bool is_movable;

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string SpriteName { get => sprite_name; set => sprite_name = value; }

    public int Level { get => level; set => level = value; }
    public int AbilityPoint { get => ability_point; set => ability_point = value; }

    public int CurEXP { get => cur_exp; set => cur_exp = value; }
    public int NextEXP { get => next_exp; set => next_exp = value; }
    public int CurHP { get => cur_hp; set => cur_hp = value; }
    public int MaxHP { get => max_hp; set => max_hp = value; }

    public int Vitality { get => vitality; set => vitality = value; }
    public int Mobility { get => mobility; set => mobility = value; }
    public int Strength { get => strength; set => strength = value; }
    public int Action { get => action; set => action = value; }

    public string MoveSkill { get => skills[0]; }
    public List<string> Skills { get => skills; set => skills = value; }
    public StringIntDictionary SkillWaitingTime { get => skill_waiting_time; set => skill_waiting_time = value; }
    public StringIntDictionary SkillLevel { get => skill_level; set => skill_level = value; }

    public List<string> Artifacts { get => artifacts; set => artifacts = value; }
    public StringIntDictionary Artifact_count { get => artifact_count; set => artifact_count = value; }

    public List<string> StateEffects { get => state_effects; set => state_effects = value; }
    public StringIntDictionary State_effect_count { get => state_effect_count; set => state_effect_count = value; }

    public Vector2Int Position { get => new Vector2Int(position_x, position_y); set { position_x = value.x; position_y = value.y; } }
    public UnitAlliance Alliance { get => alliance; set => alliance = value; }
    public Sprite Sprite { get => DataManager.instance.SpriteDictionary[SpriteName]; }
    public bool IsSkillable { get => is_skillable; set => is_skillable = value; }
    public bool IsMovable { get => is_movable; set => is_movable = value; }
    public int SkillCount { get => skill_count_this_turn; set => skill_count_this_turn = value; }
    public int MoveCount { get => move_count_this_turn; set => move_count_this_turn = value; }
}