using UnityEngine;
using UnityEngine.EventSystems;
using Model;

namespace View.UI
{

    public class SkillButton : PixelButton
    {

        public Skill CurrentSkill => currentSkill;
        Skill currentSkill = null;
        Unit currentUnit = null;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            SkillButton[] skillButtons = UnitControlView.instance.skillButtons;
            foreach(var skillButton in skillButtons)
                skillButton.SetInteractable(!skillButton.pressed);

            SetInteractable(true);

            if (pressed)
                View.IndicatorView.ShowSkillIndicator(currentUnit, currentSkill);
            else
                View.IndicatorView.HideTileIndicator();
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
        }

        public override void SetInteractable(bool boolean)
        {
            properties.interactable = boolean && CurrentSkill != null && CurrentSkill.IsUsable(currentUnit);
            FrameImage.color = properties.interactable ? Color.white : Color.grey;
        }

    }

}