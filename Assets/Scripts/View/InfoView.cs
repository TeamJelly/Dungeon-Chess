using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.UI;
using Model;

namespace View
{
    public class InfoView : MonoBehaviour
    {
        public static InfoView instance;

        public GameObject infoPanel;
        public UnitInfo unitInfo;
        public SimpleInfo tileInfo;
        public SimpleInfo simpleInfo;

        private void Awake()
        {
            instance = this;
        }

        public static void Hide()
        {
            InitNull();
            instance.infoPanel.SetActive(false);
        }

        public static void InitNull()
        {
            instance.unitInfo.SetNone();
            instance.tileInfo.Set("None", "None", null, Color.black, "None");
            instance.simpleInfo.Set("None", "None", null, Color.black, "None");
        }

        public static void Show(Unit unit)
        {
            instance.unitInfo.SetUnit(unit);
        }
        public static void Show(Tile tile)
        {
            instance.tileInfo.Set(tile);
        }
        public static void Show(Skill skill)
        {
            instance.simpleInfo.Set(skill);
        }
        public static void Show(Unit user, Skill skill)
        {
            instance.simpleInfo.Set(skill.Name, skill.Type, skill.Sprite, skill.Color, skill.GetDescription(user));
        }
        public static void Show(Effect effect)
        {
            instance.simpleInfo.Set(effect);
        }
        public static void Show(Obtainable obtainable)
        {
            instance.simpleInfo.Set(obtainable);
        }


    }
}
