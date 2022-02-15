﻿using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

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
            transform.DOMove(Camera.main.WorldToScreenPoint(unitPosition + Vector3.up * 0.5f), 0.1f);
        }

        public async Task<int> SetValue(int value)
        {
            await slider.DOValue(value, 0.2f).AsyncWaitForCompletion();

            return value;
        }
    }
}