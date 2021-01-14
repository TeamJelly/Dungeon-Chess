﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public CommonImage unitImage;

    public CommonText description;

    Unit allocatedUnit;
    public void Set(Unit unit)
    {
        allocatedUnit = unit;
        SetText( unit.name +
           "\nHP:" + unit.currentHP + "/" + unit.maxHP +
            "\nSPEED: " + unit.agility);
        
        unitImage.Sprite = unit.GetComponent<SpriteRenderer>().sprite;
    }

    void SetText(string text)
    {
        //Debug.Log(description.Text);
        description.Text = text;
    }

    public void Init()
    {
        gameObject.AddComponent<Button>().onClick.AddListener(() =>
        {
            UnitDescriptionUI.instance.Enable(allocatedUnit);
        });
    }
}