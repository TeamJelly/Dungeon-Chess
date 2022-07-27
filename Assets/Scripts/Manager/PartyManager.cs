using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    public static PartyManager instance;

    public int partyMoney;
    public List<Unit> partyUnits = new List<Unit>();
    public List<Skill> partySkills = new List<Skill>();
    public List<Artifact> partyArtifacts = new List<Artifact>();

    public void AddPartyUnit(Unit unit)
    {
        if (partyUnits.Contains(unit)) return;
        partyUnits.Add(unit);
        unit.Alliance = UnitAlliance.Party;
    }

    public void RemovePartyUnit(Unit unit)
    {
        if (!partyUnits.Contains(unit)) return;
        partyUnits.Remove(unit);
    }
}