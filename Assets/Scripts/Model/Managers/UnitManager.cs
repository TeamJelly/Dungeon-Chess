using System.Collections.Generic;
using Common;
using Model.Units;
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
            AllUnits.Add(new Proto_Priest());
            AllUnits.Add(new Proto_Warrior());
        }
    }
}