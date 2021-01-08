using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public Image unitImage;

    public TextMeshProUGUI description;

    Unit allocatedUnit;
    public void Set(Unit unit)
    {
        allocatedUnit = unit;
        SetText( unit.name +
           "\nHP:" + unit.currentHP + "/" + unit.maxHP +
            "\nSPEED: " + unit.agility);
    }

    void SetText(string text)
    {
        description.text = text;
    }

    public void Init()
    {
        gameObject.AddComponent<Button>().onClick.AddListener(() =>
        {
            UnitDescriptionUI.instance.Enable(allocatedUnit);
        });
    }
}
