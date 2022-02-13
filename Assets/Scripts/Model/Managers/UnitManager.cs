using System.Collections.Generic;
using Common;
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
            AllUnits.Add(new Unit(UnitAlliance.Party, UnitSpecies.Human, 1));
            AllUnits.Add(new Unit(UnitAlliance.Party, UnitSpecies.Human, 1));
            // AllUnits.Add(new P001_Priest());
            // AllUnits.Add(new P000_Warrior());
        }

        public static void Reset()
        {
            instance = new UnitManager();
        }
    }
}