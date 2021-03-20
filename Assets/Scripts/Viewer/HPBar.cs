using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class HPBar : MonoBehaviour
    {
        public Slider slider;

        public void Init(Unit unit)
        {
            name = $"{unit.Name} HPBAR";
            slider.maxValue = unit.MaximumHP;
            slider.minValue = 0;
            slider.value = unit.CurrentHP;
            SetPosition(new Vector3(unit.Position.x, unit.Position.y));
        }
        public void SetPosition(Vector3 unitPosition)
        {
            transform.position = 
                Camera.main.WorldToScreenPoint(unitPosition + Vector3.up * 0.5f);
        }

        public void SetValue(int v)
        {
            slider.value = v;
        }
    }
}