using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Model;
using Model.Skills;

public class SkillButton : PixelButton
{
    public Skill CurrentSkill;
/*
    public void SetSkill(Skill skill)
    {
        CurrentSkill = skill;

        Image.sprite = CurrentSkill.Sprite;

        if (CurrentSkill.Grade == 0)
            SetInteractable(false);
        else
            SetInteractable(true);
    }

    public void SetInteractable(bool boolean)
    {
        interactable = boolean;

        if (interactable == false)
        {
            ImageTransfrom.anchoredPosition = new Vector2(0, -1);
            Frame.sprite = PushedButtonSprite[CurrentSkill.Grade];
        }
        else
        {
            ImageTransfrom.anchoredPosition = new Vector2(0, 0);
            Frame.sprite = ButtonSprite[CurrentSkill.Grade];
        }
    }

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
