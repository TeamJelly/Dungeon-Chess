using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
namespace Model.Managers
{
    public class UnitManager
    {
        //고용 가능한 모든 유닛들
        public List<Unit> AllUnits = new List<Unit>();

        static UnitManager instance;
        public static UnitManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UnitManager();
                return instance;
            }
        }

        UnitManager()
        {

        }

        /// <summary>
        /// 유닛 이름으로 유닛 반환. DB에서 불러오는게 좋을 듯 함.
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public static Unit GetUnit(string unitName)
        {
            Unit newUnit = new Unit(unitName);
            return newUnit;
        }
    }
}