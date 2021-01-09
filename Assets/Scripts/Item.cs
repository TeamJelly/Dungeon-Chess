using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public int number;
    public string name = "No Skill Name";
    public UnitClass unitClass = UnitClass.NULL;
    public SkillGrade skillGrade = SkillGrade.NULL;
}
