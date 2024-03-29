﻿using UnityEngine;
using UnityEngine.EventSystems;
using Model;
using TMPro;

namespace View.UI
{

    public class SkillButton : PixelButton
    {
        public Skill CurrentSkill => currentSkill;
        Skill currentSkill = null;
        Unit currentUnit = null;
        public TextMeshProUGUI LeftCount;

        public void Init()
        {
            toggleOption = true;
            OnPushButton = () => View.IndicatorView.ShowSkillIndicator(currentUnit, currentSkill);
            OnPopButton = () => View.IndicatorView.HideTileIndicator();
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!properties.interactable) return;
            // 상호가능함을 반전시킨다.
            UnitControlView.instance.ToggleAllButtons();

            // 나 자신은 상호 가능하게 한다.
            SetInteractable(true);
        }

        public void SetSkill(Unit unit, Skill skill)
        {
            currentSkill = skill;
            currentUnit = unit;

            if (skill == null)
            {
                MainImage.sprite = null; // 나중에 빈 이미지로 교체하기
                MainImage.color = Color.clear;
                return;
            }

            MainImage.sprite = CurrentSkill.Sprite;
            MainImage.color = Color.white;
        }

        public override void SetInteractable(bool boolean)
        {
            if (currentSkill != null && currentSkill.WaitingTime != 0)
                LeftCount.text = (currentSkill.WaitingTime).ToString();
            else
                LeftCount.text = "";

            properties.interactable = boolean && CurrentSkill != null && CurrentSkill.IsUsable();
            FrameImage.color = properties.interactable ? Color.white : Color.grey;
        }

    }

}