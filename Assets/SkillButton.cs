using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Model;
using Model.Skills;
using UnityEngine.Events;
using System;

public class SkillButton : PixelButton
{

    public Skill CurrentSkill => currentSkill;
    Skill currentSkill = null;
    Unit currentUnit = null;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        //Debug.Log("Pointer Down");
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        //Debug.Log("Pointer Up");
    }

    
    public void SetSkill(Unit unit, Skill skill)
    {
        currentSkill = skill;
        currentUnit = unit;

        if (skill == null)
        {
            MainImage.sprite = null; // 나중에 빈 이미지로 교체하기
            return;
        }
        MainImage.sprite = CurrentSkill.Sprite;
        properties.onClick.AddListener(() =>
        {
            if (pressed)
                 UI.Battle.IndicatorUI.ShowTileIndicator(unit, skill);
            else UI.Battle.IndicatorUI.HideTileIndicator();
        });
        
    }
    
    public override void SetInteractable(bool boolean)
    {
        properties.interactable = boolean && CurrentSkill != null && CurrentSkill.IsUsable(currentUnit);
        FrameImage.color = properties.interactable ? Color.white : Color.grey;
    }

    /*
        private void Start()
        {
            UpgradeButton.onClick.AddListener(() =>
            {
                CurrentSkill?.Upgrade();
                SetSkill(CurrentSkill);
            });

            // 테스트용 코드
            // SetSkill(new Skill_000() { Grade = 1 });
        }*/
}
