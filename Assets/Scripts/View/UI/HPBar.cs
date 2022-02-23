using DG.Tweening;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class HPBar : MonoBehaviour
    {
        public Slider slider;
        public Image image;

        public static Dictionary<UnitAlliance, Color> AllianceToColorDict = new Dictionary<UnitAlliance, Color>
        {
            {UnitAlliance.NULL, Color.white },
            {UnitAlliance.Party, new Color(0.3f,0.3f,1) },
            {UnitAlliance.Neutral, new Color(1,1,0.5f) },
            {UnitAlliance.Enemy, new Color(1,0.5f,0.5f) },
            {UnitAlliance.Friendly, new Color(0.7f,0.7f,1) }
        };

        public void Init(Unit unit)
        {
            name = $"{unit.Name} HPBAR";
            slider.maxValue = unit.MaxHP;
            slider.minValue = 0;
            slider.value = unit.CurHP;

            image.color = AllianceToColorDict[unit.Alliance];

            SetPosition(new Vector3(unit.Position.x, unit.Position.y));
        }

        public void SetPosition(Vector3 unitPosition)
        {
            transform.position =
                Camera.main.WorldToScreenPoint(unitPosition + Vector3.up * 0.5f);
        }

        public int SetValue(int value)
        {
            AnimationManager.Reserve(DOTween.Sequence().Append(
                slider.DOValue(value, 0.1f)
            ));

            return value;
        }
    }
}