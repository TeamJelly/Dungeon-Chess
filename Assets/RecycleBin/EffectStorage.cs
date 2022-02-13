using System.Collections;
using UnityEngine;
using Common.DB;

//namespace Model.Effects
//{
//    public class EffectStorage : Storage<int, EffectDescriptor>
//    {
//        private static EffectStorage instance = new EffectStorage();
//        public static EffectStorage Instance => instance;
//        private EffectStorage() : base("effect_table", "number") { }
//    }
//    [System.Serializable]
//    public class EffectDescriptor : Copyable<EffectDescriptor>
//    {
//        public int number;
//        public string name;
//        public string extension = "";
//        public string description = "효과 설명";
//    }
//}