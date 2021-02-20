using System.Collections;
using UnityEngine;
using Common.DB;

namespace Model.Units
{
    public class UnitStorage : Storage<int, Unit>
    {
        private static UnitStorage instance = new UnitStorage();
        public static UnitStorage Instance => instance;
        private UnitStorage() { }
    }
}