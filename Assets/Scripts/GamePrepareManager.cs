using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePrepareManager : MonoBehaviour
{
    public List<Unit> AllUnits;
    public Unit currentUnit;
    int currentUnitIndex = 0;


    private void Awake()
    {
        UpdateCurrentUnit();
    }
    public void StartGame()
    {
        SceneLoader.MoveScene("StageScene");
    }

    public void AddToParty(Unit unit)
    {
        PartyManager.instance.AddUnit(unit);
    }

    public void SubFromParty(Unit unit)
    {
        PartyManager.instance.SubUnit(unit);
    }

    public void ShowNextUnit()
    {
        currentUnitIndex++;
        UpdateCurrentUnit();
    }

    public void ShowPrevUnit()
    {
        currentUnitIndex--;
        UpdateCurrentUnit();
    }

    //UI 업데이트도 표함.
    public void UpdateCurrentUnit()
    {
        currentUnit = AllUnits[currentUnitIndex];
        GamePrepareUI.instance.UpdateCurrentUnitImage(currentUnit.GetComponent<SpriteRenderer>().sprite);
    }

    public void HireThistUnit()
    {
        AddToParty(currentUnit);
    }
}
