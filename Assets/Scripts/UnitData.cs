using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int Seed;
    public string Name;                         // 이름
    public int level = 1;
    public int curEXP;                          // 현재 경험치
    public int nextEXP;                         // 다음 레벨업에 필요한 경험치 
    public int curHP;                           // 현재 체력
    public int maxHP;                           // 최대 체력
    public int strength;                        // 힘
    public int agility;                         // 민첩
    public int mobility;                        // 이동력
    public int criRate;                         // 치명타 율
    public float actionRate;                    // 행동가능 퍼센테이지
    public int positionX;                       // 위치X
    public int positionY;                       // 위치Y
    public List<string> skills = new List<string>();
    public List<int> skill_levels = new List<int>();
    public List<int> skill_waitingTimes = new List<int>();
    public List<string> stateEffects = new List<string>();  // 보유한 상태효과
    public List<string> belongings = new List<string>();    // 보유한 유물
    public int Alliance;
    public int Species;
    public int Modifier;
    public bool IsSkilled;
    public bool IsMoved;
    public bool IsFlying;
    public UnitData() { }
}